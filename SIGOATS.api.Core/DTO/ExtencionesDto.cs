using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class ExtencionesDto : BaseDto
    {
        public string? Nombre { get; set; }
        public string? Extencion { get; set; }
        public string? MIMEType { get; set; }
    }
}
