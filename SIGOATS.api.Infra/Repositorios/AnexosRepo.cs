using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Interfaces;

namespace SIGOATS.api.Infra.Repositorios
{
    public class AnexosRepo(ApplicationDbContext db, IStorageRepo storageRepo) : ClaseBaseService<Anexos, AnexosDto>(db)
    {
        public override IQueryable<AnexosDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Anexos
                            join a in db.Archivos on m.IdArchivo equals a.Id into archivoGroup
                            from a in archivoGroup.DefaultIfEmpty()
                            select new AnexosDto
                            {
                                Id = m.Id,
                                Nombre = m.Nombre,
                                Codigo = m.Codigo,
                                IdArchivo = m.IdArchivo,
                                MIMEType = a != null ? a.MIMEType : null,
                                IdPagina = m.IdPagina,
                                Orden = m.Orden
                            };

                return query.OrderBy(m => m.Orden).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(AnexosDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public async Task<Response<AnexosDto[], string>> GetAllById(long id)
        {
            try
            {
                var query = GetSelectBase("");
                var result = await query.Where(m => m.IdPagina == id).ToArrayAsync();

                foreach (var item in result)
                    if (!string.IsNullOrEmpty(item.MIMEType))
                        item.Formato = item.MIMEType switch
                        {
                            "application/pdf" => "PDF",
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => "XLSX",
                            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "DOCX",
                            _ => Path.GetExtension(item.MIMEType)?.TrimStart('.').ToUpperInvariant()
                        };

                return new() { Data = result };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(AnexosDto)}.{nameof(GetAllById)}: {ex.Message}", ex);
            }
        }

        public override async Task<Response<long, ResponseError>> Create(AnexosDto data)
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

                var lastImage = await db.Anexos.OrderByDescending(m => m.Orden).Select(x => x.Orden).FirstOrDefaultAsync();
                var newImage = new Anexos
                {
                    Nombre = data.Nombre,
                    Codigo = data.Codigo,
                    IdPagina = data.IdPagina,
                    IdArchivo = archivo != null ? archivo.Id : null,
                    Orden = lastImage == null ? 1 : lastImage + 1
                };
                await db.Anexos.AddAsync(newImage);
                await db.SaveChangesAsync();

                return new() { Data = newImage.Id };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public async Task<ArchivoDto> GetDoc(long id)
        {
            var query = from m in db.Anexos
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


        public async Task<Response<bool, ResponseError>> UpDown(long id, string tipo)
        {
            try
            {
                var data = await db.Anexos.FindAsync(id);
                if (data == null)
                    return new() { DataError = new ResponseError("Detalle no encontrado") };

                var ordenActual = data.Orden;

                if (tipo == "up")
                {
                    // Subir
                    if (ordenActual > 1)
                    {
                        var dataAnterior = await db.Anexos.Where(m => m.IdPagina == data.IdPagina && m.Orden < ordenActual).OrderByDescending(m => m.Orden).FirstOrDefaultAsync();
                        if (dataAnterior != null)
                        {
                            data.Orden = dataAnterior.Orden;
                            dataAnterior.Orden = ordenActual;
                            db.Anexos.Update(data);
                            db.Anexos.Update(dataAnterior);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                else if (tipo == "down")
                {
                    var dataSiguiente = await db.Anexos.Where(m => m.IdPagina == data.IdPagina && m.Orden > ordenActual).OrderBy(m => m.Orden).FirstOrDefaultAsync();
                    if (dataSiguiente != null)
                    {
                        data.Orden = dataSiguiente.Orden;
                        dataSiguiente.Orden = ordenActual;
                        db.Anexos.Update(data);
                        db.Anexos.Update(dataSiguiente);
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
