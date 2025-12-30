using Microsoft.AspNetCore.Identity;

namespace SIGOATS.api.Core.Models
{
    public class Usuarios : IdentityUser<long>
    {
        public long? RolId { get; set; }
        public string? Alias { get; set; }
        public string? Name { get; set; }
        public bool Estado { get; set; }

        public string? Document { get; set; }
        public string? TypeDocument { get; set; }

        public DateTime? UltimoLogin { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? UpdatedByUserId { get; set; }
        public string? DeletedByUserId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
