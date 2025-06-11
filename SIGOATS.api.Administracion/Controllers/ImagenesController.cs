using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class ImagenesController(ImagenesRepo repo) : GenericController<Imagenes, ImagenesDto>(repo)
    {
        [HttpGet("GetAllById/{id}")]
        public async Task<IActionResult> GetAllById(long id)
        {
            try
            {
                var response = await repo.GetAllById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
