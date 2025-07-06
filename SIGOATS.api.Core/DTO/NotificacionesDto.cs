using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class NotificacionesDto : BaseDto
    {
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public Audiencia? Audiencia { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Estado { get; set; }
        public bool Leido { get; set; }
    }
}
