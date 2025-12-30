using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIGOATS.api.Infra.Repositorios
{
    public class SeguridadRepo(ApplicationDbContext db, UserManager<Usuarios> userManager, AesEncryptionRepo aesEncryptionRepo, EmailManagerRepo email, CodigosSeguridadRepo codigosSeguridadRepo, IConfiguration configuration)
    {
        private readonly EmailManagerRepo _email = email;
        public async Task<Response<UserTokenDto, ResponseError>> Register(CredencialesDto data)
        {
            try
            {
                var userNew = new Usuarios
                {
                    Email = data.Email
                };

                var resultado = await userManager.CreateAsync(userNew, data.Password);

                if (resultado.Succeeded)
                {
                    return new() { Data = await GenerarToken(userNew) };
                }
                else
                {
                    return new() { DataError = new(string.Join(", ", resultado.Errors.Select(x => x.Description).ToArray())) };
                }
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<IdentityResult, ResponseError>> CreatePassword(NewPasswordDto data)
        {
            try
            {
                if (data.NewPass == true)
                {
                    var validacion = await ValidarId(data);

                    if (validacion.DataError != null)
                        return new() { DataError = validacion.DataError };

                    data.Id = validacion.Data;
                }

                var user = await userManager.FindByIdAsync(data.Id);

                if (user != null)
                {
                    var resultado = await userManager.AddPasswordAsync(user, data.Nuevo);

                    if (resultado.Succeeded)
                    {
                        return new() { Data = resultado };
                    }
                    else
                    {
                        var errors = resultado.Errors.FirstOrDefault(x => x.Code.Equals("UserAlreadyHasPassword"));
                        if (errors != null)
                        {
                            return new() { DataError = new("El usuario ya tiene asignada una contraseña.") };
                        }
                        else
                        {
                            return new() { DataError = new(string.Join(" ", resultado.Errors.Select(x => x.Description).ToArray())) };
                        }
                    }
                }
                else
                {
                    return new() { DataError = new("El usuario no existe.") };
                }
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<IdentityResult, ResponseError>> RecoverPassword(NewPasswordDto data)
        {
            try
            {
                if (data.NewPass == true)
                {
                    var validacion = await ValidarId(data);

                    if (validacion.DataError != null)
                        return new() { DataError = validacion.DataError };

                    data.Id = validacion.Data;
                }

                var user = await userManager.FindByIdAsync(data.Id);
                if (user != null)
                {
                    if (data.NewPass != true)
                        if (!await userManager.CheckPasswordAsync(user, data.Actual))
                            return new() { DataError = new("La contraseña actual no es correcta.") };

                    await userManager.RemovePasswordAsync(user);
                    var resultado2 = await userManager.AddPasswordAsync(user, data.Nuevo);

                    if (resultado2.Succeeded)
                    {
                        return new() { Data = resultado2 };
                    }
                    else
                    {
                        if (data.NewPass != true)
                            _ = await userManager.AddPasswordAsync(user, data.Actual);

                        return new() { DataError = new(string.Join(" ", resultado2.Errors.Select(x => x.Description).ToArray())) };
                    }
                }
                else
                {
                    return new() { DataError = new("El usuario no existe.") };
                }
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<string, ResponseError>> RememberPassword(string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                    return new() { Data = new("Si el correo es correcto, recibirá un mensaje con las instrucciones para recuperar su contraseña.") };

                var emailResult = await _email.MensajeUsuario(user, "RecoverPassword", "recoverpassword");
                if (emailResult.DataError != null)
                    return new() { DataError = emailResult.DataError };

                return new() { Data = "Si el correo es correcto, recibirá un mensaje con las instrucciones para recuperar su contraseña." };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<ResponseLoginDto, ResponseError>> Login(CredencialesDto data)
        {
            try
            {
                var user = await userManager.FindByNameAsync(data.Email) ?? await userManager.FindByEmailAsync(data.Email);

                if (user != null)
                {

                    if (await userManager.CheckPasswordAsync(user, data.Password))
                    {
                        var token = await GenerarToken(user);

                        return new()
                        {
                            Data = new()
                            {
                                Autenticacion = token,
                                Usuario = new()
                                {
                                    Id = user.Id,
                                    Name = user.Name,
                                    Email = user.Email,
                                }
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }

            return new() { DataError = new("Login Incorrecto") };
        }

        public async Task<Response<bool, ResponseError>> SecurityCode(CredencialesDto data)
        {
            try
            {
                var user = await userManager.FindByNameAsync(data.Email) ?? await userManager.FindByEmailAsync(data.Email);

                if (user != null)
                {
                    if (await userManager.CheckPasswordAsync(user, data.Password))
                    {
                        var result = await codigosSeguridadRepo.SendMail(user.Id);
                        if (result.Error)
                            return new() { DataError = result.DataError };

                        return new() { Data = true };
                    }
                }

                return new() { DataError = new("Login Incorrecto") };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<ResponseLoginDto, ResponseError>> SecurityCode(string email, string code)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                    return new() { DataError = new("Usuario no encontrado") };

                var codigoResult = await codigosSeguridadRepo.GetByIdUsuario(user.Id);
                if (codigoResult.Error)
                    return new() { DataError = codigoResult.DataError };

                var codigo = codigoResult.Data;
                if (codigo == null || codigo.Codigo != code || codigo.Fecha.Value.AddMinutes(10) < DateTime.Now)
                    return new() { DataError = new("Código de seguridad incorrecto o expirado") };

                if (codigo.Codigo == code)
                {
                    //await codigosSeguridadRepo.Delete(codigo.Id);
                    var token = await GenerarToken(user);

                    user.UltimoLogin = DateTime.Now;

                    return new()
                    {
                        Data = new()
                        {
                            Autenticacion = token,
                            Usuario = new()
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Email = user.Email,
                            }
                        }
                    };
                }
                else
                    return new() { DataError = new("Código de seguridad incorrecto") };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<bool, ResponseError>> ResendCode(string email)
        {
            try
            {
                email = HelpersRepo.Base64Decode(email);
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                    return new() { DataError = new("Usuario no encontrado") };

                var result = await codigosSeguridadRepo.SendMail(user.Id);
                if (result.Error)
                    return new() { DataError = result.DataError };

                return new() { Data = true };
            }
            catch (Exception ex)
            {
                return new() { DataError = new(ex.Message) };
            }
        }

        public async Task<Response<UserTokenDto, string>> RenovarToken(HttpContext httpContext)
        {
            var emailClaim = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == "email");

            if (emailClaim != null)
            {
                var email = emailClaim.Value;
                var user = await userManager.FindByEmailAsync(email);
                return new() { Data = await GenerarToken(user) };
            }
            else
            {
                return new() { DataError = "Token Invalido" };
            }
        }

        private async Task<UserTokenDto> GenerarToken(Usuarios usuario)
        {
            try
            {
                var claims = new List<Claim>();
                var claimsDB = await userManager.GetClaimsAsync(usuario);

                claims.AddRange(claimsDB);
                claims.Add(new("IdUsuario", usuario.Id.ToString()));

                var keyJWT = configuration["ANDOAuthOptions:ClientSecret"];
                var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyJWT));
                var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

                var expiracion = DateTime.UtcNow.AddYears(1);

                var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                    expires: expiracion, signingCredentials: creds);

                return new()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                    Expiracion = expiracion
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(SeguridadRepo)}.{nameof(GenerarToken)}: {ex.Message}", ex);
            }
        }

        private async Task<Response<string, ResponseError>> ValidarId(NewPasswordDto data)
        {
            var decodeBase64 = HelpersRepo.Base64Decode(data.Id);
            var decript = aesEncryptionRepo.Decrypt(decodeBase64);
            var values = decript.Split("&");
            if (values.Length != 2)
                return new() { DataError = new("El token no es válido.") };

            var id = values[0];
            var date = values[1];

            var fecha = DateTime.ParseExact(date, "yyyyMMddHHmmss", null);

            var fechaLimite = fecha.AddMinutes(10);

            if (DateTime.Now > fechaLimite)
                return new() { DataError = new("El enlace ha expirado. Solicite un nuevo enlace en la opción de recuperar contraseña.") };

            return new() { Data = id };
        }
    }
}
