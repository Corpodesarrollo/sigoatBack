using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeguridadController(SeguridadRepo service) : ControllerBase
    {
        private readonly SeguridadRepo _service = service;

        [HttpGet("RememberPassword/{email}")]
        public async Task<IActionResult> RememberPassword(string email)
        {
            var response = await _service.RememberPassword(email);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CredencialesDto data)
        {
            var response = await _service.Register(data);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CredencialesDto data)
        {
            var response = await _service.Login(data);
            return Ok(response);
        }

        [HttpPost("SecurityCode")]
        public async Task<IActionResult> SecurityCode(CredencialesDto data)
        {
            var response = await _service.SecurityCode(data);
            return Ok(response);
        }

        [HttpGet("SecurityCode/{email}/{code}")]
        public async Task<IActionResult> SecurityCode(string email, string code)
        {
            var response = await _service.SecurityCode(email, code);
            return Ok(response);
        }

        [HttpGet("ReenviarCodigo/{email}")]
        public async Task<IActionResult> ReenviarCodigo(string email)
        {
            var response = await _service.ResendCode(email);
            return Ok(response);
        }


        [HttpPost("CreatePassword")]
        public async Task<IActionResult> CreatePassword(NewPasswordDto data)
        {
            var response = await _service.CreatePassword(data);
            return Ok(response);
        }

        [HttpPost("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(NewPasswordDto data)
        {
            var response = await _service.RecoverPassword(data);
            return Ok(response);
        }

        [HttpPost("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RenovarToken()
        {
            var response = await _service.RenovarToken(HttpContext);
            return Ok(response);
        }
    }
}
