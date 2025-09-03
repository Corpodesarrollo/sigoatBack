using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Interfaces;
using SISPRO.TRV.Web.MVCCore;

namespace SIGOATS.api.Seguridad.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class AuthController(IAuthRepo repo) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<UserDto>> Get()
        {
            var result = new Response<UserDto, ResponseError>();
            try
            {
                var user = this.GetUser();
                var response = await repo.GetUser(user);
                return Ok(result.Data = response);
            }
            catch (Exception ex)
            {
                return Ok(result.DataError = new(ex.Message));
            }
        }

        [HttpGet("Prueba")]
        public async Task<ActionResult<UserDto>> Prueba()
        {
            var result = new Response<UserDto, ResponseError>();
            try
            {
                var user = this.GetUser();
                var response = await repo.GetUser(user);
                return Ok(result.Data = response);
            }
            catch (Exception ex)
            {
                return Ok(result.DataError = new(ex.Message));
            }
        }
    }
}
