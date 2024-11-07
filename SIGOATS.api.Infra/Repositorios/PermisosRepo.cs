using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class PermisosRepo(ApplicationDbContext db) : ClaseBaseService<Permisos, PermisosDto>(db)
    {
    }
}
