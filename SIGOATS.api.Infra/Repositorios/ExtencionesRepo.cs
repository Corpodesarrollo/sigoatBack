using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class ExtencionesRepo(ApplicationDbContext db) : ClaseBaseService<Extenciones, ExtencionesDto>(db)
    {
        public override IQueryable<ExtencionesDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Extenciones
                            select new ExtencionesDto
                            {
                                Id = m.Id,
                                Nombre = m.Nombre,
                                Extencion = m.Extencion,
                                MIMEType = m.MIMEType,
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Nombre.Contains(search) || m.Extencion.Contains(search));

                return query.OrderBy(m => m.Nombre).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(ExtencionesDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
