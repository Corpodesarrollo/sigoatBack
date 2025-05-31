using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Contactenos : BaseEntity
    {
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Asunto { get; set; }
        public string? Mensaje { get; set; }
    }
}
