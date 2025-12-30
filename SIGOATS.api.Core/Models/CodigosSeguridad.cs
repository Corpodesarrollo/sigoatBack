namespace SIGOATS.api.Core.Models
{
    public class CodigosSeguridad
    {
        public long Id { get; set; }
        public long? IdUsuario { get; set; }
        public string? Codigo { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
