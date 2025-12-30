using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class EmailsRepo(ApplicationDbContext db) : ClaseBaseService<Emails, EmailsDto>(db)
    {
        private readonly ApplicationDbContext db = db;

        public override IQueryable<EmailsDto> GetSelectBase(string? search = null)
        {
            try
            {
                var selectQuery = from m in db.Emails
                                  select new EmailsDto
                                  {
                                      Id = m.Id,
                                      Codigo = m.Codigo,
                                      Descripcion = m.Descripcion,
                                      CC = m.CC,
                                      CCO = m.CCO,
                                      Subject = m.Subject,
                                      Body = m.Body,
                                      Variables = (from ev in db.EmailsVariables
                                                   join v in db.Variables on ev.VariableId equals v.Id
                                                   where ev.EmailId == m.Id
                                                   select new VariablesDto { Key = v.Key, Value = v.Value }).ToList(),
                                      IdUsuarioEdicion = m.IdUsuarioEdicion,
                                      FechaEdicion = m.FechaEdicion
                                  };

                return selectQuery;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<EmailsDto>().AsQueryable();
            }
        }

        public override async Task<Response<EmailsDto, ResponseError>> GetByID(long id)
        {
            try
            {
                var query = GetSelectBase();
                var resultado = await query.FirstOrDefaultAsync(x => x.Id == id);

                if (resultado != null)
                    return new() { Data = resultado };
                else
                    return new() { DataError = new("Email no existe.") };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<EmailsDto, ResponseError>> GetByCode(string code)
        {
            try
            {
                var query = GetSelectBase();
                var resultado = await query.FirstOrDefaultAsync(x => x.Codigo.Equals(code));

                if (resultado != null)
                    return new() { Data = resultado };
                else
                    return new() { DataError = new("No hay emails registrados.") };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public override async Task<Response<long, ResponseError>> Create(EmailsDto data)
        {
            try
            {
                var email = await db.Emails.FirstOrDefaultAsync(x => x.Codigo.ToLower().Equals(data.Codigo.ToLower()));

                if (email == null)
                {
                    email = new()
                    {
                        Codigo = data.Codigo,
                        Descripcion = data.Descripcion,
                        CC = data.CC,
                        CCO = data.CCO,
                        Subject = data.Subject,
                        Body = data.Body
                    };

                    db.Emails.Add(email);
                    await db.SaveChangesAsync();

                    return new() { Data = email.Id };
                }
                else
                {
                    return new() { DataError = new("El email ya existe.") };
                }

            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public override async Task<Response<bool, ResponseError>> Update(EmailsDto data)
        {
            try
            {
                var email = await db.Emails.FirstOrDefaultAsync(x => x.Codigo.ToLower().Equals(data.Codigo.ToLower()) && x.Id != data.Id);

                if (email == null)
                {
                    email = await db.Emails.FindAsync(data.Id);

                    if (email != null)
                    {
                        email.Codigo = data.Codigo;
                        email.Descripcion = data.Descripcion;
                        email.CC = data.CC;
                        email.CCO = data.CCO;
                        email.Subject = data.Subject;
                        email.Body = data.Body;
                        email.FechaEdicion = DateTime.Now;

                        db.Entry(email).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        return new() { Data = true };
                    }
                }

                return new() { DataError = new("El email ya existe.") };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }
    }
}
