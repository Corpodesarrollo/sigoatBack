using Microsoft.AspNetCore.Mvc;

namespace SIGOATS.api.Infra.Common
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class GenericController<T1, T2>(ClaseBaseService<T1, T2> service) : ControllerBase where T1 : class where T2 : class
    {
        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var response = await service.GetAll();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetByID(int id)
        {
            var response = await service.GetByID(id);
            return Ok(response);
        }

        [HttpGet("OnDemand")]
        public virtual async Task<IActionResult> OnDemand(int page, int pageSize, string? search)
        {
            try
            {
                var response = await service.OnDemandAsync(page, pageSize, service.GetSelectBase(search));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post(T2 data)
        {
            var response = await service.Create(data);
            return Ok(response);
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put(T2 data)
        {
            var response = await service.Update(data);
            return Ok(response);
        }

        [HttpPut("ActivateToDeactivate/{id}")]
        public virtual async Task<IActionResult> ActivateToDeactivate(long id)
        {
            var response = await service.ActivateToDeactivate(id);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(long id)
        {
            var response = await service.Delete(id);
            return Ok(response);
        }
    }
}
