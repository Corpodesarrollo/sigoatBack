namespace SIGOATS.api.Core.DTO
{
    public class PaginaNoticiaDto
    {
        public long? IdPagina { get; set; }
        public long? IdNoticia { get; set; }
        public string? Titulo { get; set; }
        public string? Resumen { get; set; }
        public string? Enlace { get; set; }
        public string? Target { get; set; }
        public int? Posicion { get; set; }
        public long? IdImagen { get; set; }
        public string? UrlRecurso { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Orden { get; set; }

        public NoticiasDetallesDto[]? Detalles { get; set; }
    }
}
