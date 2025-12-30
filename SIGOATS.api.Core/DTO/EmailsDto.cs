using System.ComponentModel.DataAnnotations;

namespace SIGOATS.api.Core.DTO
{
    public class EmailsDto
    {
        public long Id { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? Codigo { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? Descripcion { get; set; }

        [Display(Name = "CC")]
        public string? CC { get; set; }

        [Display(Name = "CCO")]
        public string? CCO { get; set; }

        [Display(Name = "Asunto")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? Subject { get; set; }

        [Display(Name = "Cuerpo")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? Body { get; set; }

        [Display(Name = "Variables")]
        public List<VariablesDto>? Variables { get; set; }

        public string? IdUsuarioEdicion { get; set; }
        public DateTime? FechaEdicion { get; set; }
    }
}
