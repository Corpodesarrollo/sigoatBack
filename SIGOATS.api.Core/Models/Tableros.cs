using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Tableros : BaseEntity
    {
        public string? Titulo { get; set; }
        public string? Url { get; set; }
        public bool Estado { get; set; }
    }
}
