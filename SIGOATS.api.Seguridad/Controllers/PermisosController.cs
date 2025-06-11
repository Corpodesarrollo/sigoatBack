using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class PermisosController(PermisosRepo repo) : GenericController<Permisos, PermisosDto>(repo)
    {
        [HttpGet("Rol/{idRol}/idRol")]
        public async Task<IActionResult> Rol(long idRol)
        {
            var response = await repo.Rol(idRol);
            return Ok(response);
        }

        [HttpGet("AllByRol/{idRol}/idRol")]
        public async Task<IActionResult> AllByRol(long idRol)
        {
            var response = await repo.AllByRol(idRol);
            return Ok(response);
        }

        [HttpPut("Active")]
        public async Task<IActionResult> Active(ActivarPermisoDto data)
        {
            var response = await repo.Active(data);
            return Ok(response);
        }
    }
}
