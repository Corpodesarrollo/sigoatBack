using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class FooterNormatividad : BaseEntity
    {
        public string? Titulo { get; set; }

        public string? Url { get; set; }

        public bool Estado { get; set; }

        public bool Destacado { get; set; }

        public int Orden { get; set; }
    }
}
