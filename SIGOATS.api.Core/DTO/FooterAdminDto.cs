namespace SIGOATS.api.Core.DTO
{
    public class FooterAdminDto
    {
        public List<FooterFaqDto> Faqs { get; set; } = new();

        public List<FooterNormatividadDto> Normatividad { get; set; } = new();

        public FooterInformacionInstitucionalDto InformacionInstitucional { get; set; } = new();
    }
}

