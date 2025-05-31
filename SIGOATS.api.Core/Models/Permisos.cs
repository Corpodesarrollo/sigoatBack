using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Permisos : BaseEntity
    {
        public long IdRol { get; set; }
        public long IdModulo { get; set; }
        public bool Crear { get; set; }
        public bool Consultar { get; set; }
        public bool Editar { get; set; }
        public bool Eliminar { get; set; }
    }
}
