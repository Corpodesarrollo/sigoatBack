using System.ComponentModel.DataAnnotations;

namespace SIGOATS.api.Core.Common
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? UpdatedByUserId { get; set; }
        public string? DeletedByUserId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
