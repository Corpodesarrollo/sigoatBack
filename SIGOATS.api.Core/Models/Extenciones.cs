using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Extenciones : BaseEntity
    {
        public string? Nombre { get; set; }
        public string? Extencion { get; set; }
        public string? MIMEType { get; set; }
    }
}
