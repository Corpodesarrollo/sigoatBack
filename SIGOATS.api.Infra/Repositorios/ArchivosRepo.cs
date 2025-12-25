using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Interfaces;

namespace SIGOATS.api.Infra.Repositorios
{
    public class ArchivosRepo(ApplicationDbContext db, IStorageRepo storageRepo) : ClaseBaseService<Archivos, ArchivosDto>(db)
    {
        public override IQueryable<ArchivosDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Archivos
                            select new ArchivosDto
                            {
                                Id = m.Id,
                                Nombre = m.Nombre,
                                Extension = m.Extension,
                                MIMEType = m.MIMEType
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Nombre.Contains(search));

                return query.OrderBy(m => m.Nombre).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(ModulosDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public async Task<ArchivoDto> GetImg(long id)
        {
            try
            {
                var archivo = await db.Archivos.FindAsync(id);
                if (archivo == null)
                    throw new Exception("Archivo no encontrado");

                var fileData = await storageRepo.DownloadFileAsync(archivo.Nombre);
                return new ArchivoDto
                {
                    FileName = archivo.Nombre,
                    FileExtension = archivo.MIMEType,
                    File = fileData
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(ArchivosDto)}.{nameof(GetImg)}: {ex.Message}", ex);
            }
        }
    }
}
