using System.ComponentModel.DataAnnotations;

namespace SIGOATS.api.Core.DTO
{
    public class NewPasswordDto
    {
        public string? Id { get; set; }

        [Display(Name = "Contraseña actual")]
        public string? Actual { get; set; }


        [Display(Name = "Nueva contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Nuevo { get; set; }

        [Display(Name = "Repetir nueva contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Repetir { get; set; }

        public bool? NewPass { get; set; }
    }
}
