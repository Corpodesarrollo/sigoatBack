using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class ContactenosDto : BaseDto
    {
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Asunto { get; set; }
        public string? Mensaje { get; set; }
    }
}
