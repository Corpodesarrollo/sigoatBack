using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using System.Net.Mail;

namespace SIGOATS.api.Infra.Repositorios
{
    public class EmailManagerRepo(
        VariablesRepo variablesService,
        ConfigSMTPRepo configSMTPService)
    {
        private readonly VariablesRepo _variablesService = variablesService;
        private readonly ConfigSMTPRepo _configSMTPService = configSMTPService;

        private async Task<Response<bool, ResponseError>> SendEmail(EmailToSendDto emailToSend)
        {
            try
            {
                if (!emailToSend.IsValid)
                    return new() { DataError = new($"No se ha configurado correctamente el email a enviar. {emailToSend.ErrorMessage}") };

                var from = emailToSend.FromMask == null ?
                    new MailAddress(emailToSend.From ?? "") :
                     new MailAddress(emailToSend.From ?? "", emailToSend.FromMask);

                using MailMessage email = new() { From = from };

                if (emailToSend.To != null)
                    foreach (var item in emailToSend.To)
                        email.To.Add(item);

                if (emailToSend.CC != null)
                    foreach (var item in emailToSend.CC)
                        email.CC.Add(item);

                if (emailToSend.CCO != null)
                    foreach (var item in emailToSend.CCO)
                        email.Bcc.Add(item);

                if (emailToSend.Attachments != null && emailToSend.Attachments.Length != 0)
                    foreach (var item in emailToSend.Attachments)
                    {
                        var file = item.File;
                        email.Attachments.Add(new Attachment(new MemoryStream(file), $"{item.FileName}.{item.FileExtension}"));
                    }

                email.Subject = emailToSend.Subject;
                email.Body = emailToSend.Body;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;

                using var smtp = await _configSMTPService.GetConfig();
                if (smtp == null)
                    return new() { DataError = new("No se ha configurado el SMTP.") };

                smtp.Send(email);

                emailToSend.Status = true;
            }
            catch (Exception ex)
            {
                emailToSend.Status = false;
                emailToSend.ErrorMessage = ex.Message;
            }

            if (emailToSend.Status)
                return new() { Data = true };
            else
                return new() { DataError = new(emailToSend.ErrorMessage) };
        }

        public async Task<Response<bool, ResponseError>> MensajeUsuario(Usuarios usuario, string code, string point)
        {
            try
            {
                var emailResult = await _variablesService.LoadData(code, usuario, point: point);
                if (emailResult.DataError != null)
                    return new() { DataError = emailResult.DataError };

                var emailToSend = emailResult.Data;
                var result = await SendEmail(emailToSend);
                if (result.DataError != null)
                    return new() { DataError = result.DataError };

                return new() { Data = result.Data };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<bool, ResponseError>> MensajeCodigoSeguridad(string destino, string codigoSeguridad)
        {
            try
            {
                var emailResult = await _variablesService.LoadData("CodigoSeguridad", destino, codigoSeguridad);
                if (emailResult.DataError != null)
                    return new() { DataError = emailResult.DataError };

                var emailToSend = emailResult.Data;
                var result = await SendEmail(emailToSend);

                if (result.DataError != null)
                    return new() { DataError = result.DataError };
                return new() { Data = result.Data };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }
    }
}
