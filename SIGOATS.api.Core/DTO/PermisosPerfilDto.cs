namespace SIGOATS.api.Core.DTO
{
    public class PermisosPerfilDto
    {
        public long Id { get; set; }
        public long? IdRol { get; set; }
        public long IdMenu { get; set; }
        public string? NombreMenu { get; set; }
        public string? NombreMenuPadre { get; set; }
        public long? IdMenuPadre { get; set; }
        public string? NombreModulo { get; set; }
        public string? Path { get; set; }
        public string? Grupo { get; set; }
        public int? Orden { get; set; }
        public bool Crear { get; set; }
        public bool Consultar { get; set; }
        public bool Editar { get; set; }
        public bool Eliminar { get; set; }
    }
}
