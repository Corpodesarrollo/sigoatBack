using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Infra.Interfaces;
using SISPRO.TRV.Entity;

namespace SIGOATS.api.Infra.Repositorios
{
    public class AuthRepo(ApplicationDbContext db) : IAuthRepo
    {
        public async Task<UserDto?> GetUser(User data)
        {
            var user = await db.Usuarios.FirstOrDefaultAsync(x => x.Alias == data.Alias && x.Estado);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.Id,
                RolId = user.RolId,
                Alias = data.Alias,
                Email = data.Email,
                Name = data.calFullName,
                Estado = true,
                RolCode = data.UserGroups.Select(x => x.Code).ToArray(),
                EnterpriseCode = data.Enterprise.Code,
                EnterpriseDeptoCode = data.Enterprise.DeptoCode,
                EnterpriseEmail = data.Enterprise.Email,
                EnterpriseName = data.Enterprise.Name,
                EnterpriseIdentification = data.Enterprise.Identification.Number,
                IsMinSalud = data.Enterprise.Identification.IsEP2 || data.Enterprise.Identification.IsNITMinSalud,
                IsAuth = true
            };
        }
    }
}
