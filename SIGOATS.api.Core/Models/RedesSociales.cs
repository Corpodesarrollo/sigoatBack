using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class RedesSociales : BaseEntity
    {
        public long IdTipoRedSocial { get; set; }
        public string? Url { get; set; }
        public long? IdImagen { get; set; }
    }
}
