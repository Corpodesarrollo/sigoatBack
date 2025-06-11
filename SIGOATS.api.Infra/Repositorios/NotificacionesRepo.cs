using SIGOATS.api.Core.Common;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class NotificacionesRepo(ApplicationDbContext db) : ClaseBaseService<Notificaciones, NotificacionesDto>(db)
    {
        public override IQueryable<NotificacionesDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Notificaciones
                            select new NotificacionesDto
                            {
                                Id = m.Id,
                                Titulo = m.Titulo,
                                Contenido = m.Contenido,
                                TipoEvento = (TipoEvento)(m.TipoEvento ?? 0),
                                Audiencia = (Audiencia)(m.Audiencia ?? 0)
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Titulo.Contains(search));

                return query.OrderBy(m => m.Titulo).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(NotificacionesDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
