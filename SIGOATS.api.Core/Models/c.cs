using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Noticias : BaseEntity
    {
        public long? IdPagina { get; set; }
        public string? Titulo { get; set; }
        public string? Detalle { get; set; }
        public DateTime? Fecha { get; set; }
        public bool Estado { get; set; }
    }
}
