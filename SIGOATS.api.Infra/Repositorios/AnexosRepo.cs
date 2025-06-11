using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class AnexosRepo(ApplicationDbContext db) : ClaseBaseService<Anexos, AnexosDto>(db)
    {
        public override IQueryable<AnexosDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Anexos
                            join a in db.Archivos on m.IdArchivo equals a.Id
                            select new AnexosDto
                            {
                                Id = m.Id,
                                Nombre = m.Nombre,
                                Codigo = m.Codigo,
                                IdPagina = m.IdPagina,
                                IdArchivo = m.IdArchivo,
                                Archivo = a.Nombre,
                                MIMEType = a.MIMEType,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Nombre.Contains(search));

                return query.OrderBy(m => m.Nombre).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(AnexosDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }
    }
}
