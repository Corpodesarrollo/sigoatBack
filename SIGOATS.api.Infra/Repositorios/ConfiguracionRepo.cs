using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Interfaces;

namespace SIGOATS.api.Infra.Repositorios
{
    public class ConfiguracionRepo(ApplicationDbContext db, ArchivosRepo archivosRepo, IStorageRepo storageRepo) : ClaseBaseService<Configuracion, ConfiguracionDto>(db)
    {
        public override IQueryable<ConfiguracionDto> GetSelectBase(string? search = null)
        {
            try
            {
                var query = from m in db.Configuracion

                            join a2 in db.Archivos on m.IdLogoIzquierdo equals a2.Id into a2Group
                            from a2 in a2Group.DefaultIfEmpty()

                            join a3 in db.Archivos on m.IdLogoDerecho equals a3.Id into a3Group
                            from a3 in a3Group.DefaultIfEmpty()
                            where m.IsDeleted != false
                            select new ConfiguracionDto
                            {
                                Id = m.Id,
                                RedesSociales = m.RedesSociales,
                                ColorGovCo = m.ColorGovCo,
                                ColorPrincipal = m.ColorPrincipal,
                                IdLogoIzquierdo = m.IdLogoIzquierdo,
                                LogoIzquierdo = a2 != null ? new ArchivoDto { FileName = a2.Nombre } : null,
                                IdLogoDerecho = m.IdLogoDerecho,
                                LogoDerecho = a3 != null ? new ArchivoDto { FileName = a3.Nombre } : null
                            };

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(ConfiguracionDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public override async Task<Response<ConfiguracionDto, ResponseError>> GetFirst()
        {
            try
            {
                var query = GetSelectBase();
                var entity = await query.FirstOrDefaultAsync();
                return new() { Data = entity };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public override async Task<Response<long, ResponseError>> Create(ConfiguracionDto data)
        {
            try
            {
                Archivos? archivo2 = null;
                if (data.LogoIzquierdo != null || data.LogoIzquierdo?.File != null)
                {
                    archivo2 = new Archivos
                    {
                        Nombre = Guid.NewGuid().ToString(),
                        MIMEType = data?.LogoIzquierdo?.FileExtension,
                        Extension = Path.GetExtension(data?.LogoIzquierdo?.FileName)
                    };
                    await db.Archivos.AddAsync(archivo2);
                    await db.SaveChangesAsync();
                    var resulStorage = await storageRepo.UploadFileAsync(data.LogoIzquierdo.File, archivo2.Nombre, true);
                }

                Archivos? archivo3 = null;
                if (data.LogoDerecho != null || data.LogoDerecho?.File != null)
                {
                    archivo3 = new Archivos
                    {
                        Nombre = Guid.NewGuid().ToString(),
                        MIMEType = data?.LogoDerecho?.FileExtension,
                        Extension = Path.GetExtension(data?.LogoDerecho?.FileName)
                    };
                    await db.Archivos.AddAsync(archivo3);
                    await db.SaveChangesAsync();
                    var resulStorage = await storageRepo.UploadFileAsync(data.LogoDerecho.File, archivo3.Nombre, true);
                }

                var newItem = new Configuracion
                {
                    RedesSociales = data.RedesSociales,
                    ColorGovCo = data.ColorGovCo,
                    ColorPrincipal = data.ColorPrincipal,
                    IdLogoIzquierdo = archivo2 != null ? archivo2.Id : 0,
                    IdLogoDerecho = archivo3 != null ? archivo3.Id : 0
                };

                await db.Configuracion.AddAsync(newItem);
                await db.SaveChangesAsync();

                return new() { Data = newItem.Id };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public override async Task<Response<bool, ResponseError>> Update(ConfiguracionDto data)
        {
            try
            {
                Archivos? archivo2 = null;
                if (data.LogoIzquierdo != null || data.LogoIzquierdo?.File != null)
                {
                    archivo2 = await db.Archivos.FindAsync(data.IdLogoIzquierdo);
                    if (archivo2 != null)
                    {
                        archivo2.MIMEType = data?.LogoIzquierdo?.FileExtension;
                        archivo2.Extension = Path.GetExtension(data?.LogoIzquierdo?.FileName);
                        db.Archivos.Update(archivo2);
                        await db.SaveChangesAsync();
                        var resulStorage = await storageRepo.UploadFileAsync(data.LogoIzquierdo.File, archivo2.Nombre, true);
                    }
                }

                Archivos? archivo3 = null;
                if (data.LogoDerecho != null || data.LogoDerecho?.File != null)
                {
                    archivo3 = await db.Archivos.FindAsync(data.IdLogoDerecho);
                    if (archivo3 != null)
                    {
                        archivo3.MIMEType = data?.LogoDerecho?.FileExtension;
                        archivo3.Extension = Path.GetExtension(data?.LogoDerecho?.FileName);
                        db.Archivos.Update(archivo3);
                        await db.SaveChangesAsync();
                        var resulStorage = await storageRepo.UploadFileAsync(data.LogoDerecho.File, archivo3.Nombre, true);
                    }
                    else
                    {
                        archivo3 = new Archivos
                        {
                            Nombre = Guid.NewGuid().ToString(),
                            MIMEType = data?.LogoDerecho?.FileExtension,
                            Extension = Path.GetExtension(data?.LogoDerecho?.FileName)
                        };
                        await db.Archivos.AddAsync(archivo3);
                        await db.SaveChangesAsync();
                        var resulStorage = await storageRepo.UploadFileAsync(data.LogoDerecho.File, archivo3.Nombre, true);
                    }
                }

                var existingItem = await db.Configuracion.FindAsync(data.Id);
                if (existingItem == null)
                    return new() { DataError = new($"Configuracion with ID {data.Id} not found.") };

                existingItem.RedesSociales = data.RedesSociales;
                existingItem.ColorGovCo = data.ColorGovCo;
                existingItem.ColorPrincipal = data.ColorPrincipal;
                existingItem.IdLogoIzquierdo = data.IdLogoIzquierdo;
                existingItem.IdLogoDerecho = data.IdLogoDerecho;

                db.Configuracion.Update(existingItem);
                await db.SaveChangesAsync();

                return new() { Data = true };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public async Task<byte[]> GetImg(string name)
        {
            try
            {
                var file = await storageRepo.DownloadFileAsync(name);
                return file ?? Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(ConfiguracionRepo)}.{nameof(GetImg)}: {ex.Message}", ex);
            }
        }
    }
}
