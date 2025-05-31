using SIGOATS.api.Core.DTO;
using SISPRO.TRV.Entity;

namespace SIGOATS.api.Infra.Interfaces
{
    public interface IAuthRepo
    {
        Task<UserDto?> GetUser(User data);
    }
}
