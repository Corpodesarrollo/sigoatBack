using System.ComponentModel.DataAnnotations;

namespace SIGOATS.api.Core.DTO
{
    public class CredencialesDto(string email = "", string password = "")
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Email { get; set; } = email;

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Password { get; set; } = password;
    }
}
