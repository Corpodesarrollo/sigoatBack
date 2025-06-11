using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class NoticiasRepo(ApplicationDbContext db) : ClaseBaseService<Noticias, NoticiasDto>(db)
    {
        public override IQueryable<NoticiasDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Noticias
                            select new NoticiasDto
                            {
                                Id = m.Id,
                                IdPagina = m.IdPagina,
                                Titulo = m.Titulo,
                                Detalle = m.Detalle,
                                Fecha = m.Fecha,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Titulo.Contains(search));

                return query.OrderBy(m => m.Titulo).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(NoticiasDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
