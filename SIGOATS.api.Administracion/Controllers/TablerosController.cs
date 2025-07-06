using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class TablerosController(TablerosRepo repo) : GenericController<Tableros, TablerosDto>(repo)
    {
        [HttpGet("{id}/{idRol}")]
        public async Task<IActionResult> GetTablero(long id, long idRol)
        {
            try
            {
                var result = await repo.GetTablero(id, idRol);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<TablerosDto, string> { DataError = ex.Message });
            }
        }

    }
}
