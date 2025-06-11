using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class RolesRepo(ApplicationDbContext db) : ClaseBaseService<Roles, RolesDto>(db)
    {
        public override IQueryable<RolesDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Roles
                            select new RolesDto
                            {
                                Id = m.Id,
                                Codigo = m.Codigo,
                                Nombre = m.Nombre,
                                Descripcion = m.Descripcion,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Nombre.Contains(search) || m.Codigo.Contains(search));

                return query.OrderBy(m => m.Nombre).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(RolesDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
