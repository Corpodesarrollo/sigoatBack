using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class PaginasDto : BaseDto
    {
        public string? Idioma { get; set; }
        public string? Titulo { get; set; }
        public string? Detalle { get; set; }
        public string? Url { get; set; }
        public bool Estado { get; set; }
    }
}
