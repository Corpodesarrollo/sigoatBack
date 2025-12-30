namespace SIGOATS.api.Core.DTO
{
    public class CodigosSeguridadDto
    {
        public long Id { get; set; }
        public long? IdUsuario { get; set; }
        public string? Codigo { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
