using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class UsersRepo(ApplicationDbContext db, UserManager<Usuarios> _userManager, EmailManagerRepo email) : ClaseBaseService<Usuarios, UserDto>(db)
    {
        public override IQueryable<UserDto> GetSelectBase(string? search)
        {
            try
            {
                var query = from m in db.Usuarios
                            select new UserDto
                            {
                                Id = m.Id,
                                RolId = m.RolId,
                                Alias = m.Alias,
                                Email = m.Email,
                                Name = m.Name,
                                Estado = m.Estado
                            };

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(m => m.Name.Contains(search) || m.Email.Contains(search) || m.Alias.Contains(search));

                return query.OrderBy(m => m.Name).AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in {nameof(RolesDto)}.{nameof(GetSelectBase)}: {ex.Message}", ex);
            }
        }

        public async Task<bool> BloquearUsuariosInactivos()
        {
            try
            {
                var configuracion = await db.Configuracion.FirstOrDefaultAsync();
                if (configuracion == null || configuracion.DiasSinActividad == null)
                    return false;

                var diasInactividad = configuracion.DiasSinActividad.Value;
                var fechaLimite = DateTime.UtcNow.AddDays(-diasInactividad);
                var usuariosInactivos = await db.Usuarios
                    .Where(u => u.Estado == true &&
                                u.UltimoLogin != null &&
                                u.UltimoLogin < fechaLimite)
                    .ToListAsync();

                foreach (var usuario in usuariosInactivos)
                {
                    usuario.Estado = false;
                    db.Entry(usuario).State = EntityState.Modified;
                }
                if (usuariosInactivos.Count > 0)
                {
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override async Task<Response<long, ResponseError>> Create(UserDto data)
        {
            try
            {
                //validar si el usuario ya existe
                var userExist = await db.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == data.Email.ToLower());
                if (userExist != null)
                    return new() { DataError = new("El usuario ya existe.") };

                var user = new Usuarios
                {
                    RolId = data.RolId,
                    Alias = data.Alias,
                    Name = data.Name,
                    UserName = data.Email.ToLower(),
                    Email = data.Email.ToLower(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Estado = true
                };

                var resultado = await _userManager.CreateAsync(user);
                if (resultado.Succeeded)
                {
                    //enviar email de bienvenida
                    var emailResult = await email.MensajeUsuario(user, "UserAssignment", "createpassword");
                    if (emailResult.DataError != null)
                        return new() { DataError = new("Se creo el usuario correctamente pero hubo un error al enviar el correo de asignación de usuario.") };

                    return new() { Data = user.Id };
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

        public override async Task<Response<bool, ResponseError>> Update(UserDto data)
        {
            try
            {
                var user = db.Users.FirstOrDefault(x => x.Id == data.Id);

                if (user != null)
                {
                    user.RolId = data.RolId;
                    user.Alias = data.Alias;
                    user.Name = data.Name;
                    user.Email = data.Email.ToLower();
                    user.UserName = data.Email.ToLower();
                    user.Estado = data.Estado;

                    var resultado = await _userManager.UpdateAsync(user);
                    if (!resultado.Succeeded)
                        return new() { DataError = new(string.Join(", ", resultado.Errors.Select(x => x.Description).ToArray())) };

                    return new() { Data = true };
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

    }
}
