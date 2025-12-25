using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class ArchivosController(ArchivosRepo repo) : GenericController<Archivos, ArchivosDto>(repo)
    {
        [HttpGet("GetImg/{id}")]
        public async Task<IActionResult> GetImg(long id)
        {
            try
            {
                var response = await repo.GetImg(id);
                return File(response.File, response.FileExtension);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
