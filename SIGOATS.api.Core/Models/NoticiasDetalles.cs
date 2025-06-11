using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.Models
{
    public class NoticiasDetalles : BaseEntity
    {
        public long? IdNoticia { get; set; }
        public int? Tipo { get; set; } // 1: Texto, 2: Imagen, 3: Anexo
        public string? Contenido { get; set; }
        public string? Url { get; set; }
        public long? IdArchivo { get; set; } // Para imágenes y anexos
    }
}
