using SIGOATS.api.Core.Common;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class NoticiasDetallesRepo(ApplicationDbContext db) : ClaseBaseService<NoticiasDetalles, NoticiasDetallesDto>(db)
    {
        public override IQueryable<NoticiasDetallesDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.NoticiasDetalles
                            join a in db.Archivos on m.IdArchivo equals a.Id into archivoGroup
                            from a in archivoGroup.DefaultIfEmpty()
                            select new NoticiasDetallesDto
                            {
                                Id = m.Id,
                                IdNoticia = m.IdNoticia,
                                Tipo = (TipoItem)(m.Tipo ?? 0),
                                Contenido = m.Contenido,
                                Url = m.Url,
                                IdArchivo = m.IdArchivo,
                                Archivo = a != null ? $"{a.Nombre}.{a.Extension}" : null
                            };

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(NoticiasDetallesDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
