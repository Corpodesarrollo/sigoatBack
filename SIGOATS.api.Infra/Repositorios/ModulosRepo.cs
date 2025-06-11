using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class ModulosRepo(ApplicationDbContext db) : ClaseBaseService<Modulos, ModulosDto>(db)
    {
        public override IQueryable<ModulosDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Modulos
                            select new ModulosDto
                            {
                                Id = m.Id,
                                Nombre = m.Nombre,
                                Path = m.Path
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Nombre.Contains(search) || m.Path.Contains(search));

                return query.OrderBy(m => m.Nombre).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(ModulosDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
