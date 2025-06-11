using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class RedesSocialesDto : BaseDto
    {
        public long IdTipoRedSocial { get; set; }
        public string? Url { get; set; }
        public long? IdImagen { get; set; }
    }
}
