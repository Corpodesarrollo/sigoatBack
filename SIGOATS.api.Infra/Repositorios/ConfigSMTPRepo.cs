using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Infra.Common;
using System.Net;
using System.Net.Mail;

namespace SIGOATS.api.Infra.Repositorios
{
    public class ConfigSMTPRepo(ApplicationDbContext db)
    {

        private IQueryable<ConfigSMTPDto> GetSelectBase()
        {
            try
            {
                var query = from s in db.ConfigSMTP
                            select new ConfigSMTPDto
                            {
                                Id = s.Id,
                                Host = s.Host,
                                Port = s.Port,
                                User = s.User,
                                Password = s.Password,
                                DefaultCredentials = s.DefaultCredentials,
                                EnableSSL = s.EnableSSL,
                                Mask = s.Mask
                            };

                return query;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ConfigSMTPDto>().AsQueryable();
            }
        }

        public async Task<Response<ConfigSMTPDto, ResponseError>> Get()
        {
            try
            {
                var query = GetSelectBase();
                var resultado = await query.FirstOrDefaultAsync();
                if (resultado == null)
                    return new() { DataError = new("No se ha configurado el SMTP.") };
                else
                    return new() { Data = resultado };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<SmtpClient?> GetConfig()
        {
            try
            {
                var query = GetSelectBase();
                var config = await query.FirstOrDefaultAsync();

                if (config != null)
                {
                    return new()
                    {
                        Host = config.Host,
                        Port = config.Port,
                        EnableSsl = config.EnableSSL,
                        UseDefaultCredentials = config.DefaultCredentials,
                        Credentials = new NetworkCredential(config.User, config.Password),
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
