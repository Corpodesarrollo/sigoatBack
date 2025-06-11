using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class Anexos : BaseEntity
    {
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public long? IdPagina { get; set; }
        public long? IdArchivo { get; set; }
        public bool Estado { get; set; }
    }
}
