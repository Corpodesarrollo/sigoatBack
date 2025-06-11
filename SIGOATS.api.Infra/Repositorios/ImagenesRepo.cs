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
                                Url = m.Url
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
                    IdArchivo = archivo != null ? archivo.Id : null,
                    Url = data.Url,
                    Orden = lastImage == null ? 1 : lastImage + 1
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
                var fileExtension = Path.GetExtension(fileName);

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
                // tipo = "up" or "down"
                /*
                 se debe cambiar el orden de la imagen con id = id, subiendo o bajando una posición y actulizar el orden de las demás imágenes
                 */
                var imagen = await db.Imagenes.FindAsync(id);
                if (imagen == null)
                    return new() { DataError = new ResponseError("Imagen no encontrada") };

                var ordenActual = imagen.Orden;
                var imagenes = await db.Imagenes.Where(m => m.IdPagina == imagen.IdPagina).OrderBy(m => m.Orden).ToListAsync();

                if (tipo == "up")
                {
                    // Subir
                    if (ordenActual > 1)
                    {
                        var imagenAnterior = imagenes.FirstOrDefault(m => m.Orden == ordenActual - 1);
                        if (imagenAnterior != null)
                        {
                            imagen.Orden--;
                            imagenAnterior.Orden++;
                            db.Imagenes.Update(imagen);
                            db.Imagenes.Update(imagenAnterior);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                else if (tipo == "down")
                {
                    // Bajar
                    if (ordenActual < imagenes.Count)
                    {
                        var imagenSiguiente = imagenes.FirstOrDefault(m => m.Orden == ordenActual + 1);
                        if (imagenSiguiente != null)
                        {
                            imagen.Orden++;
                            imagenSiguiente.Orden--;
                            db.Imagenes.Update(imagen);
                            db.Imagenes.Update(imagenSiguiente);
                            await db.SaveChangesAsync();
                        }
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
