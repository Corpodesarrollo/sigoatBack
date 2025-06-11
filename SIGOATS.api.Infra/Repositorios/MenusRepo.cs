using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class MenusRepo(ApplicationDbContext db) : ClaseBaseService<Menus, MenusDto>(db)
    {
        public override IQueryable<MenusDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Menus

                            join mp in db.Menus on m.IdMenu equals mp.Id into menuParent
                            from mp in menuParent.DefaultIfEmpty()

                            join mo in db.Modulos on m.IdModulo equals mo.Id into modulo
                            from mo in modulo.DefaultIfEmpty()

                            select new MenusDto
                            {
                                Id = m.Id,
                                Nombre = m.Nombre,
                                Grupo = m.Grupo,
                                IdMenu = m.IdMenu,
                                Menu = mp != null ? mp.Nombre : null,
                                IdModulo = m.IdModulo,
                                Modulo = mo != null ? mo.Nombre : null,
                                Orden = m.Orden,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Nombre.Contains(search) || m.Grupo.Contains(search));

                return query.OrderBy(m => m.Nombre).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(MenusRepo)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
