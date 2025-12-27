using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class FooterFaqDto : BaseDto
    {
        public string? Pregunta { get; set; }
        public string? Respuesta { get; set; }
        public bool Estado { get; set; }
        public bool Destacado { get; set; }
        public int Orden { get; set; }
    }
}
