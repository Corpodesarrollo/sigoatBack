using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Roles : BaseEntity
    {
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
