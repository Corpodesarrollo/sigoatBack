using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class NoticiasDto : BaseDto
    {
        public long? IdPagina { get; set; }
        public string? Titulo { get; set; }
        public string? Detalle { get; set; }
        public DateTime? Fecha { get; set; }
        public bool Estado { get; set; }
    }
}
