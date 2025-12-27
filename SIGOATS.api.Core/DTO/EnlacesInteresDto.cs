using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class EnlacesInteresDto : BaseDto
    {
        public long IdPagina { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? Url { get; set; }
        public string? Target { get; set; }
        public int? Orden { get; set; }
        public bool? Estado { get; set; }
    }
}
