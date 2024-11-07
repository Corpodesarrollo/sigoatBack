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
            return response.Result();
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetByID(int id)
        {
            var response = await service.GetByID(id);
            return response.Result();
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post(T2 data)
        {
            var response = await service.Create(data);
            return response.Result();
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put(T2 data)
        {
            var response = await service.Update(data);
            return response.Result();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var response = await service.Delete(id);
            return response.Result();
        }
    }
}
