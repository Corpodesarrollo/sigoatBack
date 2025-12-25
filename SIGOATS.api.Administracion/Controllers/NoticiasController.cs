using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class NoticiasController(NoticiasRepo repo) : GenericController<Noticias, NoticiasDto>(repo)
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


        [HttpGet("PaginaNoticia/{id}")]
        public async Task<IActionResult> PaginaNoticia(long id)
        {
            try
            {
                var response = await repo.PaginaNoticia(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Noticia/{idNoticia}")]
        public async Task<IActionResult> Noticia(long idNoticia)
        {
            try
            {
                var response = await repo.Noticia(idNoticia);
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
