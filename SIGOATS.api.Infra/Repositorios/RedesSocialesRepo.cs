using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class RedesSocialesRepo(ApplicationDbContext db) : ClaseBaseService<RedesSociales, RedesSocialesDto>(db)
    {
        public override IQueryable<RedesSocialesDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.RedesSociales
                            select new RedesSocialesDto
                            {
                                Id = m.Id,
                                IdTipoRedSocial = m.IdTipoRedSocial,
                                Url = m.Url,
                                IdImagen = m.IdImagen,
                            };

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(RedesSocialesDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
