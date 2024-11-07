using SIGOATS.api.Core.DTO;
using SISPRO.TRV.Entity;

namespace SIGOATS.api.Core.Interfaces
{
    public interface IAuthRepo
    {
        Task<UserDto?> GetUser(User data);
    }
}
