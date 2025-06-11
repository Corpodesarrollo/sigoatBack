using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class MenusDto : BaseDto
    {
        public string? Nombre { get; set; }
        public string? Grupo { get; set; }
        public long? IdMenu { get; set; }
        public string? Menu { get; set; }
        public long? IdModulo { get; set; }
        public string? Modulo { get; set; }
        public int? Orden { get; set; }
        public bool Estado { get; set; }
    }
}
