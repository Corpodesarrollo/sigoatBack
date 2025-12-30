namespace SIGOATS.api.Core.DTO
{
    public class ResponseLoginDto
    {
        public UserTokenDto? Autenticacion { get; set; }
        public UserDto? Usuario { get; set; }
    }
}
