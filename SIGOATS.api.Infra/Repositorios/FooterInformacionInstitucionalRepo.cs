using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Interfaces;

namespace SIGOATS.api.Infra.Repositorios
{
    public class FooterInformacionInstitucionalRepo(ApplicationDbContext db, IStorageRepo storageRepo) : ClaseBaseService<FooterInformacionInstitucional, FooterInformacionInstitucionalDto>(db)
    {
        public override IQueryable<FooterInformacionInstitucionalDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.FooterInformacionInstitucional
                            select new FooterInformacionInstitucionalDto
                            {
                                Id = m.Id,
                                Direccion = m.Direccion,
                                Telefonos = m.Telefonos,
                                Horarios = m.Horarios,
                                Correos = m.Correos
                            };

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(FooterInformacionInstitucionalDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public override async Task<Response<long, ResponseError>> Create(FooterInformacionInstitucionalDto data)
        {
            try
            {
                Archivos? archivo = null;
                if (data.LogoOficial != null || data.LogoOficial?.File != null)
                {
                    archivo = new Archivos
                    {
                        Nombre = Guid.NewGuid().ToString(),
                        MIMEType = data.MIMEType,
                        Extension = Path.GetExtension(data?.LogoOficial?.FileName)
                    };
                    await db.Archivos.AddAsync(archivo);
                    await db.SaveChangesAsync();

                    var resulStorage = await storageRepo.UploadFileAsync(data.LogoOficial.File, archivo.Nombre, true);
                }

                var newData = new FooterInformacionInstitucional
                {
                    Direccion = data.Direccion,
                    Telefonos = data.Telefonos,
                    Horarios = data.Horarios,
                    Correos = data.Correos,
                    IdLogoOficial = archivo?.Id,
                    ColorFuentePrimaria = data.ColorFuentePrimaria,
                    ColorFuenteSecundaria = data.ColorFuenteSecundaria,
                    ColorPrimario = data.ColorPrimario,
                    ColorSecundario = data.ColorSecundario,
                    Tipografia = data.Tipografia,
                    EnlaceContactenos = data.EnlaceContactenos,
                    EnlaceFacebook = data.EnlaceFacebook,
                    EnlaceInstagram = data.EnlaceInstagram,
                    EnlaceTwitter = data.EnlaceTwitter,
                    EnlaceYouTube = data.EnlaceYouTube
                };
                await db.FooterInformacionInstitucional.AddAsync(newData);
                await db.SaveChangesAsync();

                return new() { Data = newData.Id };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public override async Task<Response<bool, ResponseError>> Update(FooterInformacionInstitucionalDto data)
        {
            try
            {
                var existingData = await db.FooterInformacionInstitucional.FindAsync(data.Id);
                if (existingData == null)
                    return new() { DataError = new($"FooterInformacionInstitucional with ID {data.Id} not found.") };

                var newData = await db.Archivos.FindAsync(existingData.IdLogoOficial);
                if (data.LogoOficial != null && data.LogoOficial.File != null)
                {
                    if (newData == null)
                    {
                        newData = new Archivos
                        {
                            Nombre = Guid.NewGuid().ToString(),
                            MIMEType = data.MIMEType,
                            Extension = Path.GetExtension(data?.LogoOficial?.FileName)
                        };
                        await db.Archivos.AddAsync(newData);
                        await db.SaveChangesAsync();
                        existingData.IdLogoOficial = newData.Id;
                    }
                    else
                    {
                        newData.MIMEType = data.MIMEType;
                        newData.Extension = Path.GetExtension(data?.LogoOficial?.FileName);
                        db.Archivos.Update(newData);
                    }
                    var resulStorage = await storageRepo.UploadFileAsync(data.LogoOficial.File, newData.Nombre, true);
                }

                existingData.Direccion = data.Direccion;
                existingData.Telefonos = data.Telefonos;
                existingData.Horarios = data.Horarios;
                existingData.Correos = data.Correos;
                existingData.ColorFuentePrimaria = data.ColorFuentePrimaria;
                existingData.ColorFuenteSecundaria = data.ColorFuenteSecundaria;
                existingData.ColorPrimario = data.ColorPrimario;
                existingData.ColorSecundario = data.ColorSecundario;
                existingData.Tipografia = data.Tipografia;
                existingData.EnlaceContactenos = data.EnlaceContactenos;
                existingData.EnlaceFacebook = data.EnlaceFacebook;
                existingData.EnlaceInstagram = data.EnlaceInstagram;
                existingData.EnlaceTwitter = data.EnlaceTwitter;
                existingData.EnlaceYouTube = data.EnlaceYouTube;

                db.FooterInformacionInstitucional.Update(existingData);
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
            var query = from m in db.Archivos
                        where m.Id == id
                        select new ArchivoDto
                        {
                            FileName = m.Nombre,
                            FileExtension = m.MIMEType,
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
