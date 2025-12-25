using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Noticias : BaseEntity
    {
        public long? IdPagina { get; set; }
        public string? Titulo { get; set; }
        public string? Resumen { get; set; }
        public string? Enlace { get; set; }
        public string? Target { get; set; }
        public int? Posicion { get; set; }
        public long? IdImagen { get; set; }
        public string? UrlRecurso { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Orden { get; set; }
        public bool Estado { get; set; }
    }
}
