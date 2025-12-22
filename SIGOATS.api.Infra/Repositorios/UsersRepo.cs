using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class UsersRepo(ApplicationDbContext db) : ClaseBaseService<Usuarios, UserDto>(db)
    {
        public override IQueryable<UserDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Usuarios
                            select new UserDto
                            {
                                Id = m.Id,
                                RolId = m.RolId,
                                Alias = m.Alias,
                                Email = m.Email,
                                Name = m.Name,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Name.Contains(search) || m.Email.Contains(search) || m.Alias.Contains(search));

                return query.OrderBy(m => m.Name).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(RolesDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
