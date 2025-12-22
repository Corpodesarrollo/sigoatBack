using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class NoticiasDetallesDto : BaseDto
    {
        public long? IdNoticia { get; set; }
        public TipoItem Tipo { get; set; }
        public string? Contenido { get; set; }
        public long? IdTablero { get; set; }
        public string? Url { get; set; }
        public long? IdArchivo { get; set; }
        public ArchivoDto? Archivo { get; set; }
        public string? MIMEType { get; set; }
        public int? Orden { get; set; }
    }
}
