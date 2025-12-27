using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class EnlaceInteresRepo(ApplicationDbContext db) : ClaseBaseService<EnlacesInteres, EnlacesInteresDto>(db)
    {
        public override IQueryable<EnlacesInteresDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.EnlacesInteres
                            select new EnlacesInteresDto
                            {
                                Id = m.Id,
                                IdPagina = m.IdPagina,
                                Titulo = m.Titulo,
                                Descripcion = m.Descripcion,
                                Url = m.Url,
                                Target = m.Target,
                                Orden = m.Orden,
                                Estado = m.Estado
                            };

                return query.OrderBy(m => m.Orden).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(EnlacesInteresDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public async Task<Response<EnlacesInteresDto[], string>> GetAllById(long id)
        {
            try
            {
                var query = GetSelectBase("");
                var result = await query.Where(m => m.IdPagina == id).ToArrayAsync();

                return new() { Data = result };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(EnlacesInteresDto)}.{nameof(GetAllById)}: {ex.Message}", ex);
            }
        }

        public async Task<Response<bool, ResponseError>> UpDown(long id, string tipo)
        {
            try
            {
                var data = await db.EnlacesInteres.FindAsync(id);
                if (data == null)
                    return new() { DataError = new ResponseError("Enlace no encontrado") };

                var ordenActual = data.Orden;

                if (tipo == "up")
                {
                    // Subir
                    if (ordenActual > 1)
                    {
                        var dataAnterior = await db.EnlacesInteres.Where(m => m.IdPagina == data.IdPagina && m.Orden < ordenActual).OrderByDescending(m => m.Orden).FirstOrDefaultAsync();
                        if (dataAnterior != null)
                        {
                            data.Orden = dataAnterior.Orden;
                            dataAnterior.Orden = ordenActual;
                            db.EnlacesInteres.Update(data);
                            db.EnlacesInteres.Update(dataAnterior);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                else if (tipo == "down")
                {
                    var dataSiguiente = await db.EnlacesInteres.Where(m => m.IdPagina == data.IdPagina && m.Orden > ordenActual).OrderBy(m => m.Orden).FirstOrDefaultAsync();
                    if (dataSiguiente != null)
                    {
                        data.Orden = dataSiguiente.Orden;
                        dataSiguiente.Orden = ordenActual;
                        db.EnlacesInteres.Update(data);
                        db.EnlacesInteres.Update(dataSiguiente);
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

        public async Task<Response<EnlacesInteresDto[], string>> GetAllActiveById(long id)
        {
            try
            {
                var query = GetSelectBase("");
                var result = await query.Where(m => m.IdPagina == id && m.Estado == true).ToArrayAsync();
                return new() { Data = result };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(EnlacesInteresDto)}.{nameof(GetAllActiveById)}: {ex.Message}", ex);
            }
        }

        public override async Task<Response<long, ResponseError>> Create(EnlacesInteresDto data)
        {
            try
            {
                var last = await db.EnlacesInteres.OrderByDescending(m => m.Orden).Select(x => x.Orden).FirstOrDefaultAsync();
                var newItem = new EnlacesInteres
                {
                    Descripcion = data.Descripcion,
                    Estado = data.Estado,
                    IdPagina = data.IdPagina,
                    Target = data.Target,
                    Titulo = data.Titulo,
                    Url = data.Url,
                    Orden = last == null ? 1 : last + 1
                };
                await db.EnlacesInteres.AddAsync(newItem);
                await db.SaveChangesAsync();

                return new() { Data = newItem.Id };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }
    }
}
