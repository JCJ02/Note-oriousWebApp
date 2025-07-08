namespace Note_oriousWebApp.API.Configurations
{
    public class JWTSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string RefreshKey { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public int RefreshExpiresIn { get; set; }
    }
}
