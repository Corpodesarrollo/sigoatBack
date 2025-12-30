using System.ComponentModel.DataAnnotations;

namespace SIGOATS.api.Core.Models
{
    public class Variables
    {
        [Key]
        public long Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
