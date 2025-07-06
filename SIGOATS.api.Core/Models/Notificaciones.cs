using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Notificaciones : BaseEntity
    {
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public int? Audiencia { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Estado { get; set; }
    }
}
