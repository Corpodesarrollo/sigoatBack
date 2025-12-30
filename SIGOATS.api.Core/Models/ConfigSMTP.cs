namespace SIGOATS.api.Core.Models
{
    public class ConfigSMTP
    {
        public int Id { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? User { get; set; }
        public string? Mask { get; set; }
        public string? Password { get; set; }
        public bool EnableSSL { get; set; }
        public bool DefaultCredentials { get; set; }
    }
}
