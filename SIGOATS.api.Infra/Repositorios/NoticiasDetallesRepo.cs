using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.Common;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Interfaces;

namespace SIGOATS.api.Infra.Repositorios
{
    public class NoticiasDetallesRepo(ApplicationDbContext db, IStorageRepo storageRepo) : ClaseBaseService<NoticiasDetalles, NoticiasDetallesDto>(db)
    {
        public override IQueryable<NoticiasDetallesDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.NoticiasDetalles
                            join a in db.Archivos on m.IdArchivo equals a.Id into archivoGroup
                            from a in archivoGroup.DefaultIfEmpty()
                            orderby m.Orden
                            select new NoticiasDetallesDto
                            {
                                Id = m.Id,
                                IdNoticia = m.IdNoticia,
                                Tipo = (TipoItem)(m.Tipo ?? 0),
                                Contenido = m.Contenido,
                                Url = m.Url,
                                Orden = m.Orden,
                                MIMEType = a != null ? a.MIMEType : null,
                                IdArchivo = m.IdArchivo
                            };

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(NoticiasDetallesDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public async Task<Response<NoticiasDetallesDto[], string>> GetAllById(long id)
        {
            try
            {
                var query = GetSelectBase("");
                var result = await query.Where(m => m.IdNoticia == id).ToArrayAsync();

                return new() { Data = result };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(NoticiasDetallesDto)}.{nameof(GetAllById)}: {ex.Message}", ex);
            }
        }

        public override async Task<Response<long, ResponseError>> Create(NoticiasDetallesDto data)
        {
            try
            {
                Archivos? archivo = null;
                if (data.Tipo == TipoItem.Imagen)
                {
                    if (data.Archivo != null || data.Archivo?.File != null)
                    {
                        archivo = new Archivos
                        {
                            Nombre = Guid.NewGuid().ToString(),
                            MIMEType = data.MIMEType,
                            Extension = Path.GetExtension(data?.Archivo?.FileName)
                        };
                        await db.Archivos.AddAsync(archivo);
                        await db.SaveChangesAsync();

                        var resulStorage = await storageRepo.UploadFileAsync(data.Archivo.File, archivo.Nombre, true);
                    }
                }

                var last = await db.NoticiasDetalles.OrderByDescending(m => m.Orden).Select(x => x.Orden).FirstOrDefaultAsync();
                var newItem = new NoticiasDetalles
                {
                    IdNoticia = data.IdNoticia,
                    IdArchivo = archivo != null ? archivo.Id : null,
                    Contenido = data.Contenido,
                    Tipo = (int)data.Tipo,
                    Url = data.Url,
                    Orden = last == null ? 1 : last + 1
                };
                await db.NoticiasDetalles.AddAsync(newItem);
                await db.SaveChangesAsync();

                return new() { Data = newItem.Id };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public async Task<ArchivoDto> GetImg(long id)
        {
            var detalle = await db.NoticiasDetalles.FindAsync(id);
            if (detalle == null)
                throw new Exception($"Detalle with ID {id} not found.");

            if (!string.IsNullOrEmpty(detalle.Url))
            {
                var url = detalle.Url;
                // obtener la imagen desde la url. La imagen se encuentra en una pagina web o en un servidor externo
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(detalle.Url);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"No se pudo obtener la imagen desde la URL: {detalle.Url}");

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = Path.GetFileName(new Uri(detalle.Url).LocalPath);
                var fileExtension = "image/jpeg";

                return new ArchivoDto
                {
                    FileName = fileName,
                    FileExtension = fileExtension,
                    File = fileBytes
                };
            }
            else
            {
                var query = from m in db.NoticiasDetalles
                            join a in db.Archivos on m.IdArchivo equals a.Id
                            where m.Id == id
                            select new ArchivoDto
                            {
                                FileName = a.Nombre,
                                FileExtension = a.MIMEType,
                            };

                var result = await query.FirstOrDefaultAsync();
                if (result == null)
                    throw new Exception($"Detalle with ID {id} not found.");
                else
                {
                    var file = await storageRepo.DownloadFileAsync(result.FileName);
                    if (file == null)
                        throw new Exception($"File with name {result.FileName} not found in storage.");

                    result.File = file;

                    return result;
                }
            }
        }

        public async Task<Response<bool, ResponseError>> UpDown(long id, string tipo)
        {
            try
            {
                var detalle = await db.NoticiasDetalles.FindAsync(id);
                if (detalle == null)
                    return new() { DataError = new ResponseError("Detalle no encontrado") };

                var ordenActual = detalle.Orden;

                if (tipo == "up")
                {
                    // Subir
                    if (ordenActual > 1)
                    {
                        var detalleAnterior = await db.NoticiasDetalles.Where(m => m.IdNoticia == detalle.IdNoticia && m.Orden < ordenActual).OrderByDescending(m => m.Orden).FirstOrDefaultAsync();
                        if (detalleAnterior != null)
                        {
                            detalle.Orden = detalleAnterior.Orden;
                            detalleAnterior.Orden = ordenActual;
                            db.NoticiasDetalles.Update(detalle);
                            db.NoticiasDetalles.Update(detalleAnterior);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                else if (tipo == "down")
                {
                    var detalleSiguiente = await db.NoticiasDetalles.Where(m => m.IdNoticia == detalle.IdNoticia && m.Orden > ordenActual).OrderBy(m => m.Orden).FirstOrDefaultAsync();
                    if (detalleSiguiente != null)
                    {
                        detalle.Orden = detalleSiguiente.Orden;
                        detalleSiguiente.Orden = ordenActual;
                        db.NoticiasDetalles.Update(detalle);
                        db.NoticiasDetalles.Update(detalleSiguiente);
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
    }
}
