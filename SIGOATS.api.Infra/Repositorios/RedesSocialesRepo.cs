using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Interfaces;

namespace SIGOATS.api.Infra.Repositorios
{
    public class RedesSocialesRepo(ApplicationDbContext db, IStorageRepo storageRepo) : ClaseBaseService<RedesSociales, RedesSocialesDto>(db)
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

        public override async Task<Response<long, ResponseError>> Create(RedesSocialesDto data)
        {
            try
            {
                var archivo = new Archivos();

                if (data.Imagen != null || data.Imagen?.File != null)
                {
                    archivo = new Archivos
                    {
                        Nombre = Guid.NewGuid().ToString(),
                        MIMEType = data?.Imagen?.FileExtension,
                        Extension = Path.GetExtension(data?.Imagen?.FileName)
                    };
                    await db.Archivos.AddAsync(archivo);
                    await db.SaveChangesAsync();

                    var resulStorage = await storageRepo.UploadFileAsync(data.Imagen.File, archivo.Nombre, true);
                }

                RedesSociales item = new()
                {
                    IdImagen = archivo.Id,
                    IdTipoRedSocial = data.IdTipoRedSocial,
                    Url = data.Url
                };
                db.RedesSociales.Add(item);
                await db.SaveChangesAsync();

                return new() { Data = item.Id };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public override async Task<Response<bool, ResponseError>> Update(RedesSocialesDto data)
        {
            try
            {
                var existingItem = await db.RedesSociales.FindAsync(data.Id);
                if (existingItem == null)
                    return new() { DataError = new($"RedSocial with ID {data.Id} not found.") };

                if (data.Imagen != null && data.Imagen.File != null)
                {
                    Archivos? archivo = null;
                    if (existingItem.IdImagen != null && existingItem.IdImagen != 0)
                    {
                        archivo = await db.Archivos.FindAsync(existingItem.IdImagen);
                        if (archivo != null)
                        {
                            archivo.MIMEType = data.Imagen.FileExtension;
                            archivo.Extension = Path.GetExtension(data.Imagen.FileName);

                            db.Archivos.Update(archivo);
                        }
                        else
                        {
                            archivo = new Archivos
                            {
                                Nombre = Guid.NewGuid().ToString(),
                                MIMEType = data?.Imagen?.FileExtension,
                                Extension = Path.GetExtension(data?.Imagen?.FileName)
                            };
                            await db.Archivos.AddAsync(archivo);
                            await db.SaveChangesAsync();

                            existingItem.IdImagen = archivo.Id;
                        }
                    }
                    else
                    {
                        archivo = new Archivos
                        {
                            Nombre = Guid.NewGuid().ToString(),
                            MIMEType = data?.Imagen?.FileExtension,
                            Extension = Path.GetExtension(data?.Imagen?.FileName)
                        };
                        await db.Archivos.AddAsync(archivo);
                        await db.SaveChangesAsync();

                        existingItem.IdImagen = archivo.Id;
                    }

                    var resulStorage = await storageRepo.UploadFileAsync(data.Imagen.File, archivo.Nombre, true);
                }
                existingItem.IdTipoRedSocial = data.IdTipoRedSocial;
                existingItem.Url = data.Url;
                db.RedesSociales.Update(existingItem);
                await db.SaveChangesAsync();

                return new() { Data = true };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public async Task<ArchivoDto> GetImg(long id)
        {
            var query = from a in db.Archivos
                        where a.Id == id
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
}
