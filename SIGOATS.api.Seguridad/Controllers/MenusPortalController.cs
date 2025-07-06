using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class MenusPortalController(MenusPortalRepo repo) : GenericController<MenusPortal, MenusPortalDto>(repo) { }
}
