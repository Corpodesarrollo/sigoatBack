using SIGOATS.api.Core.DTO;
using SIGOATS.api.Infra.Interfaces;
using SISPRO.TRV.Entity;

namespace SIGOATS.api.Infra.Repositorios
{
    public class AuthRepo() : IAuthRepo
    {
        public async Task<UserDto?> GetUser(User data)
        {
            //var user = await db.ApplicationUser.FirstOrDefaultAsync(x => x.Alias == data.Alias);
            //if (user == null)
            //    return null;

            return new UserDto
            {
                //Id = user.Id,
                Alias = data.Alias,
                Email = data.Email,
                Name = data.calFullName,
                State = true,
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
