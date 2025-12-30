using System.ComponentModel.DataAnnotations;

namespace SIGOATS.api.Core.Models
{
    public class Emails
    {
        [Key]
        public long Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public string? CC { get; set; }
        public string? CCO { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? IdUsuarioEdicion { get; set; }
        public DateTime? FechaEdicion { get; set; }
    }
}
