using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class FooterInformacionInstitucionalDto : BaseDto
    {
        public string? Direccion { get; set; }
        public string? Telefonos { get; set; }
        public string? Horarios { get; set; }
        public string? Correos { get; set; }
        public string? EnlaceTwitter { get; set; }
        public string? EnlaceFacebook { get; set; }
        public string? EnlaceInstagram { get; set; }
        public string? EnlaceYouTube { get; set; }
        public string? EnlaceContactenos { get; set; }
        public long? IdLogoOficial { get; set; }
        public ArchivoDto? LogoOficial { get; set; }
        public string? MIMEType { get; set; }
        public string? ColorPrimario { get; set; }
        public string? ColorSecundario { get; set; }
        public string? Tipografia { get; set; }
        public string? ColorFuentePrimaria { get; set; }
        public string? ColorFuenteSecundaria { get; set; }
    }
}
