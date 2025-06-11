using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class AnexosDto : BaseDto
    {
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public long? IdPagina { get; set; }
        public long? IdArchivo { get; set; }
        public string? Archivo { get; set; }
        public string? MIMEType { get; set; }
        public bool Estado { get; set; }
    }
}
