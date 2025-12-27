using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class FooterNormatividadController(FooterNormatividadRepo repo) : GenericController<FooterNormatividad, FooterNormatividadDto>(repo)
    {
        [HttpGet("Activos")]
        public async Task<IActionResult> Activos()
        {
            try
            {
                var response = await repo.Activos();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpDown/{id}/{tipo}")]
        public async Task<IActionResult> UpDown(long id, string tipo)
        {
            try
            {
                var response = await repo.UpDown(id, tipo);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
