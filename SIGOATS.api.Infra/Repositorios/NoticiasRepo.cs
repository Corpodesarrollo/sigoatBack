using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.Common;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class NoticiasRepo(ApplicationDbContext db) : ClaseBaseService<Noticias, NoticiasDto>(db)
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
                                Detalle = m.Detalle,
                                Fecha = m.Fecha,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Titulo.Contains(search));

                return query.OrderBy(m => m.Titulo).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(NoticiasDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
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
                                      orderby n.Fecha descending
                                      select new PaginaNoticiaDto
                                      {
                                          IdPagina = n.IdPagina,
                                          IdNoticia = n.Id,
                                          Titulo = n.Titulo,
                                          Detalle = n.Detalle,
                                          Fecha = n.Fecha
                                      }
                                      ).ToArrayAsync();

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
                                         Detalle = n.Detalle,
                                         Fecha = n.Fecha
                                     }
                                     ).FirstOrDefaultAsync();

                if (noticia == null)
                    return new() { DataError = new("Noticia no encontrada") };

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

                return new() { Data = noticia };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(PaginaNoticiaDto)}.{nameof(Noticia)}: {ex.Message}", ex);
            }
        }
    }
}
