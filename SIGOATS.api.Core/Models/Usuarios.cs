using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Usuarios : BaseEntity
    {
        public long? RolId { get; set; }
        public string? Alias { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public bool Estado { get; set; }
    }
}
