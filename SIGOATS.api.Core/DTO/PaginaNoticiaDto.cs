namespace SIGOATS.api.Core.DTO
{
    public class PaginaNoticiaDto
    {
        public long? IdPagina { get; set; }
        public long? IdNoticia { get; set; }
        public string? Titulo { get; set; }
        public string? Detalle { get; set; }
        public DateTime? Fecha { get; set; }

        public NoticiasDetallesDto[]? Detalles { get; set; }
    }
}
