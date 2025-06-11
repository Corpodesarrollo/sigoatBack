using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Archivos : BaseEntity
    {
        public string? Nombre { get; set; }
        public string? Extension { get; set; }
        public string? MIMEType { get; set; }
    }
}
