using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class ConfiguracionDto : BaseDto
    {
        public bool RedesSociales { get; set; }
        public string? ColorGovCo { get; set; }
        public string? ColorPrincipal { get; set; }
        public long? IdLogoIzquierdo { get; set; }
        public ArchivoDto? LogoIzquierdo { get; set; }
        public long? IdLogoDerecho { get; set; }
        public ArchivoDto? LogoDerecho { get; set; }
    }
}
