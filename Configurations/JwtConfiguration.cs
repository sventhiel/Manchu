namespace Manchu.Configurations
{
    public class JwtConfiguration
    {
        public string? IssuerSigningKey { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateLifetime { get; set; }
        public string? ValidAudience { get; set; }
        public string? ValidIssuer { get; set; }
        public int ValidLifetime { get; set; }
    }
}