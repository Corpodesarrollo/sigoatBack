using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Configuracion : BaseEntity
    {
        public bool RedesSociales { get; set; }
        public string? ColorGovCo { get; set; }
        public string? ColorPrincipal { get; set; }
        public long? IdLogoIzquierdo { get; set; }
        public long? IdLogoDerecho { get; set; }
    }
}
