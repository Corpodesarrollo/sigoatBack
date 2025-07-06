using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class NotificacionesLeidasDto : BaseDto
    {
        public long? IdUsuario { get; set; }
        public long? IdNotificacion { get; set; }
        public DateTime? FechaLeido { get; set; }
    }
}
