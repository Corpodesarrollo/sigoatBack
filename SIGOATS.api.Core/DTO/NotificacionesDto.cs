using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class NotificacionesDto : BaseDto
    {
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public TipoEvento? TipoEvento { get; set; }
        public Audiencia? Audiencia { get; set; }
    }
}
