using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class PermisosDto : BaseDto
    {
        public long IdRol { get; set; }
        public long IdMenu { get; set; }
        public bool Crear { get; set; }
        public bool Consultar { get; set; }
        public bool Editar { get; set; }
        public bool Eliminar { get; set; }
    }
}
