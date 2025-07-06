using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class MenusPortalRepo(ApplicationDbContext db) : ClaseBaseService<MenusPortal, MenusPortalDto>(db)
    {
        public override IQueryable<MenusPortalDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.MenusPortal

                            join mp in db.MenusPortal on m.IdMenu equals mp.Id into menuParent
                            from mp in menuParent.DefaultIfEmpty()

                            join mo in db.Paginas on m.IdModulo equals mo.Id into modulo
                            from mo in modulo.DefaultIfEmpty()

                            select new MenusPortalDto
                            {
                                Id = m.Id,
                                Nombre = m.Nombre,
                                IdMenu = m.IdMenu,
                                Menu = mp != null ? mp.Nombre : null,
                                IdModulo = m.IdModulo,
                                Modulo = mo != null ? mo.Titulo : null,
                                Orden = m.Orden,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Nombre.Contains(search));

                return query.OrderBy(m => m.Nombre).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(MenusPortalDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
