using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class ContactenosRepo(ApplicationDbContext db) : ClaseBaseService<Contactenos, ContactenosDto>(db)
    {
        public override IQueryable<ContactenosDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Contactenos
                            select new ContactenosDto
                            {
                                Id = m.Id,
                                Email = m.Email,
                                Telefono = m.Telefono,
                                Asunto = m.Asunto,
                                Mensaje = m.Mensaje
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Email.Contains(search) || m.Telefono.Contains(search));

                return query.OrderBy(m => m.Email).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(ContactenosDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
