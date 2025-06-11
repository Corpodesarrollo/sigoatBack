using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class ExtencionesController(ExtencionesRepo repo) : GenericController<Extenciones, ExtencionesDto>(repo) { }
}
