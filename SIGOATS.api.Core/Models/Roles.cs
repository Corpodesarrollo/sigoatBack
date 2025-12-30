using Microsoft.AspNetCore.Identity;

namespace SIGOATS.api.Core.Models
{
    public class Roles : IdentityRole<long>
    {
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
