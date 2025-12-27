using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class FooterNormatividadDto : BaseDto
    {
        public string? Titulo { get; set; }
        public string? Url { get; set; }
        public bool Estado { get; set; }
        public bool Destacado { get; set; }
        public int Orden { get; set; }
    }
}
