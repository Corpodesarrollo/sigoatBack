using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class ConfiguracionController(ConfiguracionRepo repo) : GenericController<Configuracion, ConfiguracionDto>(repo)
    {
        [HttpGet("GetImg/{id}")]
        public async Task<IActionResult> GetImg(string id)
        {
            var response = await repo.GetImg(id);
            return File(response, "imagen/png");
        }
    }
}
