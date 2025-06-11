using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Imagenes : BaseEntity
    {
        public long? IdPagina { get; set; }
        public long? IdArchivo { get; set; }
        public string? Url { get; set; }
        public int? Orden { get; set; }
    }
}
