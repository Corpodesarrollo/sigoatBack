using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class EnlacesInteres : BaseEntity
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? Url { get; set; }
        public string? TipoApertura { get; set; }
        public int? Orden { get; set; }
        public bool Estado { get; set; }
    }
}
