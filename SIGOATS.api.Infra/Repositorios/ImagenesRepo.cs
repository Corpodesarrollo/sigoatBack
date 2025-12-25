using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Interfaces;

namespace SIGOATS.api.Infra.Repositorios
{
    public class ImagenesRepo(ApplicationDbContext db, IStorageRepo storageRepo) : ClaseBaseService<Imagenes, ImagenesDto>(db)
    {
        public override IQueryable<ImagenesDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Imagenes
                            join a in db.Archivos on m.IdArchivo equals a.Id into archivoGroup
                            from a in archivoGroup.DefaultIfEmpty()
                            select new ImagenesDto
                            {
                                Id = m.Id,
                                IdArchivo = m.IdArchivo,
                                MIMEType = a != null ? a.MIMEType : null,
                                IdPagina = m.IdPagina,
                                Orden = m.Orden,
                                Url = m.Url,
                                Texto = m.Texto,
                                TipoContenido = m.TipoContenido,
                                Estado = m.Estado
                            };

                return query.OrderBy(m => m.Orden).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(ImagenesDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public async Task<Response<ImagenesDto[], string>> GetAllById(long id)
        {
            try
            {
                var query = GetSelectBase("");
                var result = await query.Where(m => m.IdPagina == id).ToArrayAsync();

                return new() { Data = result };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(ImagenesDto)}.{nameof(GetAllById)}: {ex.Message}", ex);
            }
        }

        public override async Task<Response<long, ResponseError>> Create(ImagenesDto data)
        {
            try
            {
                Archivos? archivo = null;
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

                var lastImage = await db.Imagenes.OrderByDescending(m => m.Orden).Select(x => x.Orden).FirstOrDefaultAsync();
                var newImage = new Imagenes
                {
                    IdPagina = data.IdPagina,
                    IdArchivo = archivo?.Id,
                    Url = data.Url,
                    Texto = data.Texto,
                    Orden = lastImage == null ? 1 : lastImage + 1,
                    Estado = true
                };
                await db.Imagenes.AddAsync(newImage);
                await db.SaveChangesAsync();

                return new() { Data = newImage.Id };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public async Task<ArchivoDto> GetImg(long id)
        {
            var imagen = await db.Imagenes.FindAsync(id);
            if (imagen == null)
                throw new Exception($"Image with ID {id} not found.");

            if (!string.IsNullOrEmpty(imagen.Url))
            {
                var url = imagen.Url;
                // obtener la imagen desde la url. La imagen se encuentra en una pagina web o en un servidor externo
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(imagen.Url);
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"No se pudo obtener la imagen desde la URL: {imagen.Url}");

                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = Path.GetFileName(new Uri(imagen.Url).LocalPath);
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
                var query = from m in db.Imagenes
                            join a in db.Archivos on m.IdArchivo equals a.Id
                            where m.Id == id
                            select new ArchivoDto
                            {
                                FileName = a.Nombre,
                                FileExtension = a.MIMEType,
                            };

                var result = await query.FirstOrDefaultAsync();
                if (result == null)
                    throw new Exception($"Image with ID {id} not found.");
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
                var data = await db.Imagenes.FindAsync(id);
                if (data == null)
                    return new() { DataError = new ResponseError("Detalle no encontrado") };

                var ordenActual = data.Orden;

                if (tipo == "up")
                {
                    // Subir
                    if (ordenActual > 1)
                    {
                        var dataAnterior = await db.Imagenes.Where(m => m.IdPagina == data.IdPagina && m.Orden < ordenActual).OrderByDescending(m => m.Orden).FirstOrDefaultAsync();
                        if (dataAnterior != null)
                        {
                            data.Orden = dataAnterior.Orden;
                            dataAnterior.Orden = ordenActual;
                            db.Imagenes.Update(data);
                            db.Imagenes.Update(dataAnterior);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                else if (tipo == "down")
                {
                    var dataSiguiente = await db.Imagenes.Where(m => m.IdPagina == data.IdPagina && m.Orden > ordenActual).OrderBy(m => m.Orden).FirstOrDefaultAsync();
                    if (dataSiguiente != null)
                    {
                        data.Orden = dataSiguiente.Orden;
                        dataSiguiente.Orden = ordenActual;
                        db.Imagenes.Update(data);
                        db.Imagenes.Update(dataSiguiente);
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
