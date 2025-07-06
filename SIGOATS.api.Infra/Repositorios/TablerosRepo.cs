using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class TablerosRepo(ApplicationDbContext db) : ClaseBaseService<Tableros, TablerosDto>(db)
    {
        public override IQueryable<TablerosDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Tableros
                            select new TablerosDto
                            {
                                Id = m.Id,
                                Titulo = m.Titulo,
                                Url = m.Url,
                                Estado = m.Estado,
                            };

                return query.OrderBy(m => m.Titulo).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(TablerosDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public async Task<Response<TablerosDto, ResponseError>> GetTablero(long id, long idRol)
        {
            try
            {
                var tienePermiso = await (from p in db.Permisos
                                          join m in db.Menus on p.IdMenu equals m.Id
                                          where p.IdRol == idRol && m.IdTablero == id && p.Consultar == true
                                          select m).AnyAsync();

                if (!tienePermiso)
                    return new() { DataError = new("401") };

                var query = GetSelectBase("");

                var tablero = await query.FirstOrDefaultAsync(x => x.Id == id);
                if (tablero == null)
                    return new() { DataError = new("404") };

                if (!tablero.Estado)
                    return new() { DataError = new("403") };

                return new() { Data = tablero };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }
    }
}
