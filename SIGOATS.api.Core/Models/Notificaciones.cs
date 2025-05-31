using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Notificaciones : BaseEntity
    {
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public int? TipoEvento { get; set; }
        public int? Audiencia { get; set; }
    }
}
