using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class PaginasRepo(ApplicationDbContext db) : ClaseBaseService<Paginas, PaginasDto>(db)
    {
        public override IQueryable<PaginasDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Paginas
                            select new PaginasDto
                            {
                                Id = m.Id,
                                Titulo = m.Titulo,
                                Detalle = m.Detalle,
                                Idioma = m.Idioma,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Titulo.Contains(search));

                return query.OrderBy(m => m.Titulo).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(PaginasDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
