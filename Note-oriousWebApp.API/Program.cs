using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Note_oriousWebApp.API.Configurations;
using Note_oriousWebApp.API.Helpers;
using Note_oriousWebApp.API.Middlewares;
using Note_oriousWebApp.API.Repositories;
using Note_oriousWebApp.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Services to the Container.
// Auth
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<AuthService>();
// Notes
builder.Services.AddScoped<NotesRepository>();
builder.Services.AddScoped<NotesService>();
// Users
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<UsersService>();

// Register DBContext with PostgreSQL
builder.Services.AddDbContext<AppDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register JWTSettings and TokenHelper
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JWTSettings>();
builder.Services.AddSingleton<TokenHelper>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddSwaggerGen(configure =>
{
    configure.SwaggerDoc("v1", new() { Title = "Note-orious API", Version = "v1" });

    configure.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
    });

    configure.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Add Authorization
builder.Services.AddAuthorization(options =>
{
    // Optional: define named policies
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Call Helpers
//string randomString = GenerateRandomStringHelper.GenerateRandomString(16);
//Console.WriteLine($"Generated Random String: {randomString}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<AuthMiddleware>();

//app.UseAuthentication();

//app.UseAuthorization();

app.MapControllers();

app.Run();
