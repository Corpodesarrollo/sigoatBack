using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.Common;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using SIGOATS.api.Infra.Interfaces;

namespace SIGOATS.api.Infra.Repositorios
{
    public class NoticiasRepo(ApplicationDbContext db, IStorageRepo storageRepo) : ClaseBaseService<Noticias, NoticiasDto>(db)
    {
        public override IQueryable<NoticiasDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Noticias
                            select new NoticiasDto
                            {
                                Id = m.Id,
                                IdPagina = m.IdPagina,
                                Titulo = m.Titulo,
                                Resumen = m.Resumen,
                                Enlace = m.Enlace,
                                Target = m.Target,
                                Posicion = m.Posicion,
                                IdImagen = m.IdImagen,
                                UrlRecurso = m.UrlRecurso,
                                Fecha = m.Fecha,
                                Orden = m.Orden,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Titulo.Contains(search));

                return query.OrderByDescending(m => m.Orden).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(NoticiasDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public override async Task<Response<NoticiasDto, ResponseError>> GetByID(long id)
        {
            try
            {
                var query = GetSelectBase("");
                var result = await query.FirstOrDefaultAsync(m => m.Id == id);
                if (result == null)
                    return new() { DataError = new("Noticia no encontrada") };

                if (result.IdImagen != null)
                {
                    var archivo = await db.Archivos.FirstOrDefaultAsync(a => a.Id == result.IdImagen);
                    if (archivo != null)
                    {
                        result.MIMEType = archivo.MIMEType;
                        result.Imagen = new ArchivoDto
                        {
                            FileName = archivo.Nombre + archivo.Extension,
                            FileExtension = archivo.Extension,
                            File = await storageRepo.DownloadFileAsync(archivo.Nombre)
                        };
                    }
                }

                return new() { Data = result };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(NoticiasDto)}.{nameof(GetByID)}: {ex.Message}", ex);
            }

        }

        public async Task<Response<NoticiasDto[], string>> GetAllById(long id)
        {
            try
            {
                var query = GetSelectBase("");
                var result = await query.Where(m => m.IdPagina == id).ToArrayAsync();

                return new() { Data = result };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(NoticiasDto)}.{nameof(GetAllById)}: {ex.Message}", ex);
            }
        }

        public async Task<Response<PaginaNoticiaDto[], string>> PaginaNoticia(long id)
        {
            try
            {
                var noticias = await (from n in db.Noticias
                                      where n.IdPagina == id
                                      orderby n.Orden descending
                                      select new PaginaNoticiaDto
                                      {
                                          IdPagina = n.IdPagina,
                                          IdNoticia = n.Id,
                                          Titulo = n.Titulo,
                                          Resumen = n.Resumen,
                                          Enlace = n.Enlace,
                                          Target = n.Target,
                                          Posicion = n.Posicion,
                                          IdImagen = n.IdImagen,
                                          UrlRecurso = n.UrlRecurso,
                                          Fecha = n.Fecha,
                                          Orden = n.Orden
                                      }).ToArrayAsync();

                if (noticias == null || noticias.Length == 0)
                    return new() { DataError = new("No hay noticias disponibles para esta página") };

                foreach (var noticia in noticias)
                {
                    var detalles = await (from dn in db.NoticiasDetalles

                                          join a in db.Archivos on dn.IdArchivo equals a.Id into archivoGroup
                                          from a in archivoGroup.DefaultIfEmpty()

                                          where dn.IdNoticia == noticia.IdNoticia

                                          orderby dn.Orden

                                          select new NoticiasDetallesDto
                                          {
                                              Id = dn.Id,
                                              Tipo = (TipoItem)dn.Tipo,
                                              IdNoticia = dn.IdNoticia,
                                              Contenido = dn.Contenido,
                                              Url = dn.Url,
                                              IdArchivo = dn.IdArchivo,
                                              MIMEType = a != null ? a.MIMEType : "",
                                              Orden = dn.Orden
                                          }).ToArrayAsync();

                    noticia.Detalles = detalles.Length > 0 ? detalles : null;
                }

                return new() { Data = noticias };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(PaginaNoticiaDto)}.{nameof(PaginaNoticia)}: {ex.Message}", ex);
            }
        }

        public async Task<Response<PaginaNoticiaDto, string>> Noticia(long idNoticia)
        {
            try
            {
                var noticia = await (from n in db.Noticias
                                     where n.Id == idNoticia
                                     select new PaginaNoticiaDto
                                     {
                                         IdPagina = n.IdPagina,
                                         IdNoticia = n.Id,
                                         Titulo = n.Titulo,
                                         Resumen = n.Resumen,
                                         Enlace = n.Enlace,
                                         Target = n.Target,
                                         Posicion = n.Posicion,
                                         IdImagen = n.IdImagen,
                                         UrlRecurso = n.UrlRecurso,
                                         Fecha = n.Fecha,
                                         Orden = n.Orden
                                     }
                                     ).FirstOrDefaultAsync();

                if (noticia == null)
                    return new() { DataError = new("Noticia no encontrada") };

                var detalles = await (from dn in db.NoticiasDetalles

                                      join a in db.Archivos on dn.IdArchivo equals a.Id into archivoGroup
                                      from a in archivoGroup.DefaultIfEmpty()

                                      join t in db.Tableros on dn.IdTablero equals t.Id into tableroGroup
                                      from t in tableroGroup.DefaultIfEmpty()

                                      where dn.IdNoticia == noticia.IdNoticia
                                      orderby dn.Orden
                                      select new NoticiasDetallesDto
                                      {
                                          Id = dn.Id,
                                          Tipo = (TipoItem)dn.Tipo,
                                          IdNoticia = dn.IdNoticia,
                                          Contenido = dn.Contenido,
                                          IdTablero = dn.IdTablero,
                                          Url = t == null ? dn.Url : t.Url,
                                          IdArchivo = dn.IdArchivo,
                                          MIMEType = a != null ? a.MIMEType : "",
                                          Orden = dn.Orden
                                      }).ToArrayAsync();

                noticia.Detalles = detalles.Length > 0 ? detalles : null;

                return new() { Data = noticia };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(PaginaNoticiaDto)}.{nameof(Noticia)}: {ex.Message}", ex);
            }
        }

        public async Task<Response<bool, ResponseError>> UpDown(long id, string tipo)
        {
            try
            {
                var data = await db.Noticias.FindAsync(id);
                if (data == null)
                    return new() { DataError = new ResponseError("Noticia no encontrada") };

                var ordenActual = data.Orden;

                if (tipo == "up")
                {
                    // Subir
                    var dataAnterior = await db.Noticias.Where(m => m.Id != data.Id && m.IdPagina == data.IdPagina && m.Orden > ordenActual).OrderBy(m => m.Orden).FirstOrDefaultAsync();
                    if (dataAnterior != null)
                    {
                        data.Orden = dataAnterior.Orden;
                        dataAnterior.Orden = ordenActual;
                        db.Noticias.Update(data);
                        db.Noticias.Update(dataAnterior);
                        await db.SaveChangesAsync();
                    }
                }
                else if (tipo == "down")
                {
                    var dataSiguiente = await db.Noticias.Where(m => m.Id != data.Id && m.IdPagina == data.IdPagina && m.Orden <= ordenActual).OrderByDescending(m => m.Orden).FirstOrDefaultAsync();
                    if (dataSiguiente != null)
                    {
                        data.Orden = dataSiguiente.Orden;
                        dataSiguiente.Orden = ordenActual;
                        db.Noticias.Update(data);
                        db.Noticias.Update(dataSiguiente);
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

        public override async Task<Response<long, ResponseError>> Create(NoticiasDto data)
        {
            try
            {
                Archivos? archivo = null;
                if (data.Imagen != null || data.Imagen?.File != null)
                {
                    archivo = new Archivos
                    {
                        Nombre = Guid.NewGuid().ToString(),
                        MIMEType = data.MIMEType,
                        Extension = Path.GetExtension(data?.Imagen?.FileName)
                    };
                    await db.Archivos.AddAsync(archivo);
                    await db.SaveChangesAsync();

                    var resulStorage = await storageRepo.UploadFileAsync(data.Imagen.File, archivo.Nombre, true);
                }

                var last = await db.Noticias.OrderByDescending(m => m.Orden).Select(x => x.Orden).FirstOrDefaultAsync();
                var newItem = new Noticias
                {
                    Enlace = data.Enlace,
                    Estado = data.Estado,
                    Fecha = data.Fecha,
                    IdPagina = data.IdPagina,
                    Posicion = data.Posicion,
                    Resumen = data.Resumen,
                    Target = data.Target,
                    Titulo = data.Titulo,
                    UrlRecurso = data.UrlRecurso,
                    IdImagen = archivo?.Id,
                    Orden = last == null ? 1 : last + 1
                };
                await db.Noticias.AddAsync(newItem);
                await db.SaveChangesAsync();

                return new() { Data = newItem.Id };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }

        public override async Task<Response<bool, ResponseError>> Update(NoticiasDto data)
        {
            try
            {
                var entity = await db.Noticias.FindAsync(data.Id);
                if (entity == null)
                    return new() { DataError = new("Noticia no encontrada") };
                if (data.Imagen != null && data.Imagen.File != null)
                {
                    Archivos? archivo = null;
                    if (data.IdImagen != null)
                    {
                        archivo = await db.Archivos.FirstOrDefaultAsync(a => a.Id == data.IdImagen);
                        if (archivo != null)
                        {
                            archivo.MIMEType = data.MIMEType;
                            archivo.Extension = Path.GetExtension(data?.Imagen?.FileName);
                            db.Archivos.Update(archivo);
                            await db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        archivo = new Archivos
                        {
                            Nombre = Guid.NewGuid().ToString(),
                            MIMEType = data.MIMEType,
                            Extension = Path.GetExtension(data?.Imagen?.FileName)
                        };
                        await db.Archivos.AddAsync(archivo);
                        await db.SaveChangesAsync();
                        data.IdImagen = archivo.Id;
                    }
                    var resulStorage = await storageRepo.UploadFileAsync(data.Imagen.File, archivo.Nombre, true);
                }

                entity.Titulo = data.Titulo;
                entity.Resumen = data.Resumen;
                entity.Enlace = data.Enlace;
                entity.Target = data.Target;
                entity.Posicion = data.Posicion;
                entity.IdImagen = data.IdImagen;
                entity.UrlRecurso = data.UrlRecurso;
                entity.Fecha = data.Fecha;
                entity.Estado = data.Estado;
                entity.Orden = data.Orden;
                db.Noticias.Update(entity);
                await db.SaveChangesAsync();
                return new() { Data = true };
            }
            catch (Exception ex)
            {
                return new() { DataError = ProcessException(ex) };
            }
        }
    }
}
