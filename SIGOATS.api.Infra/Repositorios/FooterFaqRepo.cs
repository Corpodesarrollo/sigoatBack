using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class FooterFaqRepo(ApplicationDbContext db) : ClaseBaseService<FooterFaq, FooterFaqDto>(db)
    {
        public override IQueryable<FooterFaqDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.FooterFaq
                            select new FooterFaqDto
                            {
                                Id = m.Id,
                                Pregunta = m.Pregunta,
                                Respuesta = m.Respuesta,
                                Estado = m.Estado,
                                Destacado = m.Destacado,
                                Orden = m.Orden
                            };

                return query.OrderBy(m => m.Orden).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(FooterFaqDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public override async Task<Response<long, ResponseError>> Create(FooterFaqDto entity)
        {
            try
            {
                var maxOrden = await db.FooterFaq.MaxAsync(m => (int?)m.Orden) ?? 0;
                entity.Orden = maxOrden + 1;

                return await base.Create(entity);
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public async Task<Response<bool, ResponseError>> UpDown(long id, string tipo)
        {
            try
            {
                var data = await db.FooterFaq.FindAsync(id);
                if (data == null)
                    return new() { DataError = new ResponseError("Detalle no encontrado") };

                var ordenActual = data.Orden;

                if (tipo == "up")
                {
                    // Subir
                    if (ordenActual > 1)
                    {
                        var dataAnterior = await db.FooterFaq.Where(m => m.Orden < ordenActual).OrderByDescending(m => m.Orden).FirstOrDefaultAsync();
                        if (dataAnterior != null)
                        {
                            data.Orden = dataAnterior.Orden;
                            dataAnterior.Orden = ordenActual;
                            db.FooterFaq.Update(data);
                            db.FooterFaq.Update(dataAnterior);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                else if (tipo == "down")
                {
                    var dataSiguiente = await db.FooterFaq.Where(m => m.Orden > ordenActual).OrderBy(m => m.Orden).FirstOrDefaultAsync();
                    if (dataSiguiente != null)
                    {
                        data.Orden = dataSiguiente.Orden;
                        dataSiguiente.Orden = ordenActual;
                        db.FooterFaq.Update(data);
                        db.FooterFaq.Update(dataSiguiente);
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    return new() { DataError = new ResponseError("Tipo de operación no válido. Use 'up' o 'down'.") };
                }

                return new() { Data = true };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public async Task<Response<FooterFaqDto[], ResponseError>> Activos()
        {
            try
            {
                var query = GetSelectBase("");
                var result = await query.Where(m => m.Estado).OrderByDescending(x => x.Destacado).ToArrayAsync();
                return new() { Data = result };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }
    }
}
