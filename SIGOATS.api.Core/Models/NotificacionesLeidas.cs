using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class NotificacionesLeidas : BaseEntity
    {
        public long? IdUsuario { get; set; }
        public long? IdNotificacion { get; set; }
        public DateTime? FechaLeido { get; set; }
    }
}
