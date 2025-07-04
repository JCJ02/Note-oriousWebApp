using Microsoft.EntityFrameworkCore;
using Note_oriousWebApp.API.Configurations;
using Note_oriousWebApp.API.Repositories;
using Note_oriousWebApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<NotesRepository>();
builder.Services.AddScoped<NotesService>();

// Register DBContext with PostgreSQL
builder.Services.AddDbContext<AppDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
