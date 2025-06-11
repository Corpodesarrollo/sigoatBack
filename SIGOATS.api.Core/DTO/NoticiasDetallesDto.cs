using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class NoticiasDetallesDto : BaseDto
    {
        public long? IdNoticia { get; set; }
        public TipoItem Tipo { get; set; }
        public string? Contenido { get; set; }
        public string? Url { get; set; }
        public long? IdArchivo { get; set; }
        public string? Archivo { get; set; }
    }
}
