using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class NoticiasDetallesController(NoticiasDetallesRepo repo) : GenericController<NoticiasDetalles, NoticiasDetallesDto>(repo) { }
}
