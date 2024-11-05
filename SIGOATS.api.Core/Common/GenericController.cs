using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIGOATS.api.Core.Common
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GenericController : ControllerBase
    {
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetByID(int id)
        {
            return Ok();
        }

        [HttpGet("OnDemand")]
        public virtual async Task<IActionResult> OnDemand()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post()
        {
            return Ok();
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put()
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            return Ok();
        }
    }
}
