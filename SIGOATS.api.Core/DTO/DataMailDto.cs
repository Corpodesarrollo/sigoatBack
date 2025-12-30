namespace SIGOATS.api.Core.DTO
{
    public class DataMailDto
    {
        public EmailsDto? Email { get; set; }
        public ConfigSMTPDto? ConfigSMTP { get; set; }
        public List<VariablesDto>? Variables { get; set; }
    }
}
