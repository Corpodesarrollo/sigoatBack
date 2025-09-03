using Microsoft.EntityFrameworkCore;
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
                                Audiencia = (Audiencia)(m.Audiencia ?? 0),
                                Estado = m.Estado,
                                FechaInicio = m.FechaInicio,
                                FechaFin = m.FechaFin,

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

        public async Task<Response<NotificacionesDto[], ResponseError>> GetByUserAndRole(int idUser)
        {
            try
            {
                var query = from n in db.Notificaciones
                            join nl in db.NotificacionesLeidas on n.Id equals nl.IdNotificacion into nlGroup
                            from nl in nlGroup.DefaultIfEmpty()
                            where (nl == null || nl.IdUsuario == idUser) && n.Estado
                            select new NotificacionesDto
                            {
                                Id = n.Id,
                                Titulo = n.Titulo,
                                Contenido = n.Contenido,
                                TipoEvento = (TipoEvento)(n.TipoEvento ?? 0),
                                Audiencia = (Audiencia)(n.Audiencia ?? 0),
                                Estado = n.Estado,
                                FechaInicio = n.FechaInicio,
                                FechaFin = n.FechaFin,
                                Leido = nl != null && nl.IdUsuario == idUser,
                            };

                var notificaciones = await query.OrderByDescending(n => n.FechaInicio).Take(10).ToArrayAsync();
                return new() { Data = notificaciones };

            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public async Task<Response<NotificacionesDto[], ResponseError>> Marcar(long idNot, int idUser)
        {
            try
            {
                var notificacion = await db.Notificaciones.FindAsync(idNot);
                if (notificacion == null)
                {
                    return new Response<NotificacionesDto[], ResponseError> { DataError = new("Notificación no encontrada") };
                }
                var notificacionLeida = await db.NotificacionesLeidas.FirstOrDefaultAsync(nl => nl.IdNotificacion == idNot && nl.IdUsuario == idUser);
                if (notificacionLeida == null)
                {
                    notificacionLeida = new NotificacionesLeidas
                    {
                        IdNotificacion = idNot,
                        IdUsuario = idUser,
                        FechaLeido = DateTime.UtcNow
                    };
                    db.NotificacionesLeidas.Add(notificacionLeida);
                    await db.SaveChangesAsync();
                }


                return await GetByUserAndRole(idUser);
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }
    }
}
