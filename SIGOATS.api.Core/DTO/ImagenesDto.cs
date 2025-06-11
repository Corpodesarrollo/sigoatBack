using SIGOATS.api.Core.Common;

namespace SIGOATS.api.Core.DTO
{
    public class ImagenesDto : BaseDto
    {
        public long? IdPagina { get; set; }
        public long? IdArchivo { get; set; }
        public ArchivoDto? Archivo { get; set; }
        public string? MIMEType { get; set; }
        public string? Url { get; set; }
        public int? Orden { get; set; }
    }
}
