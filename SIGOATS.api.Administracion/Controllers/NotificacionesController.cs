using Microsoft.AspNetCore.Mvc;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Repositorios;

namespace SIGOATS.api.Seguridad.Controllers
{
    public class NotificacionesController(NotificacionesRepo repo) : GenericController<Notificaciones, NotificacionesDto>(repo)
    {
        [HttpGet("{idUser}/idUser")]
        public async Task<IActionResult> GetByUserAndRole(int idUser)
        {
            var response = await repo.GetByUserAndRole(idUser);
            return Ok(response);
        }

        [HttpGet("{idNot}/idNot/{idUser}/idUser")]
        public async Task<IActionResult> Marcar(long idNot, int idUser)
        {
            var response = await repo.Marcar(idNot, idUser);
            return Ok(response);
        }
    }
}
