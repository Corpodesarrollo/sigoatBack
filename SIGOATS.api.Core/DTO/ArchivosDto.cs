using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class ArchivosDto : BaseDto
    {
        public string? Nombre { get; set; }
        public string? Extension { get; set; }
        public string? MIMEType { get; set; }
    }
}
