using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Paginas : BaseEntity
    {
        public string? Idioma { get; set; }
        public string? Titulo { get; set; }
        public string? Detalle { get; set; }
        public string? Url { get; set; }
        public bool Estado { get; set; }
    }
}
