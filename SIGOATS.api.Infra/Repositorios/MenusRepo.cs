using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class MenusRepo(ApplicationDbContext db) : ClaseBaseService<Menus, MenusDto>(db)
    {

    }
}
