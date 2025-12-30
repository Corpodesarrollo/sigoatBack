using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class CodigosSeguridadRepo(ApplicationDbContext db, UsersRepo usuariosService, EmailManagerRepo emailManager) : ClaseBaseService<CodigosSeguridad, CodigosSeguridadDto>(db)
    {
        private readonly Mapper _mapper = MapperConfig.InitializeAutomapper();

        public override IQueryable<CodigosSeguridadDto> GetSelectBase(string? code = null)
        {
            try
            {
                var selectQuery = from c in db.CodigosSeguridad
                                  select new CodigosSeguridadDto
                                  {
                                      Id = c.Id,
                                      IdUsuario = c.IdUsuario,
                                      Codigo = c.Codigo,
                                      Fecha = c.Fecha,
                                  };
                return selectQuery;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<CodigosSeguridadDto>().AsQueryable();
            }
        }

        public async Task<Response<CodigosSeguridadDto, ResponseError>> GetByIdUsuario(long id)
        {
            try
            {
                var query = GetSelectBase().Where(x => x.IdUsuario == id);
                var entity = await query.FirstOrDefaultAsync();
                if (entity == null)
                    return new() { DataError = new("No se encontró el registro") };

                return new() { Data = entity };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<bool, ResponseError>> SendMail(long idUsuario)
        {
            try
            {
                var code = new Random().Next(0, 999999).ToString("000000");
                //busca el usuario en CodigosSeguridad, si existe lo actualiza sino lo crea
                var codigoResult = await GetByIdUsuario(idUsuario);
                if (codigoResult.Error)
                {
                    var entity = new CodigosSeguridad
                    {
                        IdUsuario = idUsuario,
                        Codigo = code,
                        Fecha = DateTime.Now
                    };

                    var result = await base.Create(_mapper.Map<CodigosSeguridadDto>(entity));
                    if (result.DataError != null)
                        return new() { DataError = result.DataError };
                }
                else
                {
                    var codigo = codigoResult.Data;
                    codigo.Codigo = code;
                    codigo.Fecha = DateTime.Now;

                    var result = await base.Update(codigo);
                    if (result.DataError != null)
                        return new() { DataError = result.DataError };
                }

                var usuarioResult = await usuariosService.GetByID(idUsuario);
                if (usuarioResult.DataError != null)
                    return new() { DataError = usuarioResult.DataError };

                var usuario = usuarioResult.Data;
                var emailResult = await emailManager.MensajeCodigoSeguridad(usuario.Email, code);
                if (emailResult.DataError != null)
                    return new() { DataError = emailResult.DataError };

                return new() { Data = true };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }
    }
}
