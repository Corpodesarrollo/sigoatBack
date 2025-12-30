using Microsoft.Extensions.Configuration;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class VariablesRepo(ApplicationDbContext db, ConfigSMTPRepo configSMTPService, AesEncryptionRepo aes, EmailsRepo emailsService, HelpersRepo helpersRepo, IConfiguration config)
    {
        private readonly ApplicationDbContext db = db;
        private readonly ConfigSMTPRepo _configSMTPService = configSMTPService;
        private readonly AesEncryptionRepo _aes = aes;
        private readonly EmailsRepo _emailsService = emailsService;
        private readonly HelpersRepo _helpersRepo = helpersRepo;
        private readonly IConfiguration _config = config;

        public async Task<Response<DataMailDto, ResponseError>> GetData(string? CodeEmail, Usuarios? usuario = null)
        {
            var emailResult = await _emailsService.GetByCode(CodeEmail);
            var smtpResult = await _configSMTPService.Get();
            var variablesResult = await GetValues(usuario);

            if (emailResult.DataError != null)
                return new() { DataError = emailResult.DataError };

            if (smtpResult.DataError != null)
                return new() { DataError = smtpResult.DataError };

            if (variablesResult.DataError != null)
                return new() { DataError = variablesResult.DataError };

            return new()
            {
                Data = new()
                {
                    Email = emailResult.Data,
                    ConfigSMTP = smtpResult.Data,
                    Variables = variablesResult.Data != null ? [.. variablesResult.Data] : []
                }
            };
        }

        public async Task<Response<VariablesDto[], ResponseError>> GetValues(Usuarios? user = null)
        {
            try
            {
                if (user != null)
                {
                    VariablesDto[] list =
                    [
                        new() { Key = "Usuario", Value = user.Name },
                        new() { Key = "EmailUsuario", Value = user.Email },
                        new() { Key = "TelefonoUsuario", Value = user.PhoneNumber }
                    ];

                    return new() { Data = list };
                }

                return new() { Data = [] };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }


        public async Task<Response<EmailToSendDto, ResponseError>> LoadData(string code, Usuarios usuario, AttachmentFileDto[]? attachments = null, string? point = null)
        {
            var dataResult = await GetData(code, usuario);
            if (dataResult.DataError != null)
                return new() { DataError = dataResult.DataError };

            var data = dataResult.Data;

            if (data.Email != null && data.ConfigSMTP != null)
            {
                var encript = HelpersRepo.Base64Encode(_aes.Encrypt($"{usuario.Id}&{DateTime.Now:yyyyMMddHHmmss}"));

                var serverHost = _config.GetSection("ServerHost").Value;

                if (data.Variables != null)
                    data.Variables.Add(new VariablesDto { Key = "Enlace", Value = $"{serverHost}/{point}/{encript}" });

                var emailToSend = new EmailToSendDto
                {
                    From = data.ConfigSMTP.User,
                    FromMask = data.ConfigSMTP.Mask,
                    To = [usuario?.Email],
                    CC = data.Email.CC?.Split(';'),
                    CCO = data.Email.CCO?.Split(';'),
                    Subject = data.Email.Subject,
                    Body = LoadVariables(data.Email.Body, data.Variables, data.Email.Variables),
                    Attachments = attachments,
                    SentDate = DateTime.Now,
                };

                return new() { Data = emailToSend };
            }
            else
                return new() { DataError = new("No se ha configurado el email de asignación de usuario.") };
        }

        public async Task<Response<EmailToSendDto, ResponseError>> LoadData(string code, string email, AttachmentFileDto[]? attachments = null)
        {
            var dataResult = await GetData(code);
            if (dataResult.DataError != null)
                return new() { DataError = dataResult.DataError };

            var data = dataResult.Data;

            if (data.Email != null && data.ConfigSMTP != null)
            {
                var emailToSend = new EmailToSendDto
                {
                    From = data.ConfigSMTP.User,
                    FromMask = data.ConfigSMTP.Mask,
                    To = [email],
                    CC = data.Email.CC?.Split(';'),
                    CCO = data.Email.CCO?.Split(';'),
                    Subject = data.Email.Subject,
                    Body = LoadVariables(data.Email.Body, data.Variables, data.Email.Variables),
                    Attachments = attachments,
                    SentDate = DateTime.Now,
                };

                return new() { Data = emailToSend };
            }
            else
                return new() { DataError = new("No se ha configurado el email de asignación de usuario.") };
        }

        public async Task<Response<EmailToSendDto, ResponseError>> LoadData(string code, string email, string securityCode)
        {
            var dataResult = await GetData(code);
            if (dataResult.DataError != null)
                return new() { DataError = dataResult.DataError };

            var data = dataResult.Data;
            data.Variables.Add(new VariablesDto { Key = "CodigoSeguridad", Value = securityCode });

            if (data.Email != null && data.ConfigSMTP != null)
            {
                var emailToSend = new EmailToSendDto
                {
                    From = data.ConfigSMTP.User,
                    FromMask = data.ConfigSMTP.Mask,
                    To = [email],
                    CC = data.Email.CC?.Split(';'),
                    CCO = data.Email.CCO?.Split(';'),
                    Subject = $"{data.Email.Subject} - {securityCode}",
                    Body = LoadVariables(data.Email.Body, data.Variables, data.Email.Variables),
                    SentDate = DateTime.Now,
                };

                return new() { Data = emailToSend };
            }
            else
                return new() { DataError = new("No se ha configurado el email de asignación de usuario.") };
        }

        public async Task<Response<EmailToSendDto, ResponseError>> LoadData(string code, string email, VariablesDto[] variables, AttachmentFileDto[]? attachments = null)
        {
            var dataResult = await GetData(code);
            if (dataResult.DataError != null)
                return new() { DataError = dataResult.DataError };

            var data = dataResult.Data;
            data.Variables.AddRange(variables);

            if (data.Email != null && data.ConfigSMTP != null)
            {
                var emailToSend = new EmailToSendDto
                {
                    From = data.ConfigSMTP.User,
                    FromMask = data.ConfigSMTP.Mask,
                    To = [email],
                    CC = data.Email.CC?.Split(';'),
                    CCO = data.Email.CCO?.Split(';'),
                    Subject = $"{data.Email.Subject}",
                    Body = LoadVariables(data.Email.Body, data.Variables, data.Email.Variables),
                    Attachments = attachments,
                    SentDate = DateTime.Now,
                };

                return new() { Data = emailToSend };
            }
            else
                return new() { DataError = new("No se ha configurado el email de asignación de usuario.") };
        }

        private static string LoadVariables(string? body, List<VariablesDto> origen, List<VariablesDto>? destino)
        {
            if (body == null || origen == null || destino == null)
                return string.Empty;

            destino.ForEach(item =>
            {
                var variable = origen.FirstOrDefault(x => x.Key.Equals(item.Key));
                if (variable != null)
                    body = body.Replace($"[{item.Key}]", variable.Value);
            });

            return body;
        }
    }
}
