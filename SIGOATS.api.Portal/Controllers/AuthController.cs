using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
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
            var user = this.GetUser();
            var result = await repo.GetUser(user);

            if (result == null)
                return BadRequest();

            return Ok(result);
        }
    }
}
