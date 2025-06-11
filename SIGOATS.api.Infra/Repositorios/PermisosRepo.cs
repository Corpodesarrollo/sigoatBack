using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
using SIGOATS.api.Infra.Common;

namespace SIGOATS.api.Infra.Repositorios
{
    public class PermisosRepo(ApplicationDbContext db) : ClaseBaseService<Permisos, PermisosDto>(db)
    {
        public async Task<Response<PermisosPerfilDto[], string>> Rol(long idPerfil)
        {
            try
            {
                var resultado = await (from menu in db.Menus

                                       join menuP in db.Menus on menu.IdMenu equals menuP.Id into leftMenu
                                       from menuPadre in leftMenu.DefaultIfEmpty()

                                       join modulo in db.Modulos on menu.IdModulo equals modulo.Id into left
                                       from cl in left.DefaultIfEmpty()

                                       join permiso in db.Permisos on new { a = menu.Id, c = idPerfil } equals new { a = permiso.IdMenu, c = permiso.IdRol } into ps
                                       from l in ps.DefaultIfEmpty()

                                       select new PermisosPerfilDto
                                       {
                                           IdRol = idPerfil,
                                           IdMenu = menu.Id,
                                           NombreMenu = menu.Nombre,
                                           NombreModulo = cl != null ? cl.Nombre : "",
                                           Path = cl != null ? cl.Path : "",
                                           IdMenuPadre = menuPadre == null ? null : menuPadre.Id,
                                           NombreMenuPadre = menuPadre == null ? "" : menuPadre.Nombre,
                                           Grupo = menu.Grupo,
                                           Orden = menu.Orden,
                                           Crear = l != null ? l.Crear : false,
                                           Consultar = l != null ? l.Consultar : false,
                                           Editar = l != null ? l.Editar : false,
                                           Eliminar = l != null ? l.Eliminar : false,
                                       })
                                       .OrderBy(x => x.Grupo)
                                       .ThenBy(x => x.Orden)
                                       .ToArrayAsync();

                return new() { Data = resultado };
            }
            catch (Exception ex)
            {
                return new() { DataError = ex.Message };
            }
        }

        public async Task<Response<PermisosPerfilDto[], string>> AllByRol(long idPerfil)
        {
            try
            {
                var resultado = await (from menu in db.Menus

                                       join menuP in db.Menus on menu.IdMenu equals menuP.Id into leftMenu
                                       from menuPadre in leftMenu.DefaultIfEmpty()

                                       join modulo in db.Modulos on menu.IdModulo equals modulo.Id into left
                                       from cl in left.DefaultIfEmpty()

                                       join permiso in db.Permisos on new { a = menu.Id, c = idPerfil } equals new { a = permiso.IdMenu, c = permiso.IdRol } into ps
                                       from l in ps.DefaultIfEmpty()

                                       select new PermisosPerfilDto
                                       {
                                           IdRol = idPerfil,
                                           IdMenu = menu.Id,
                                           NombreMenu = menu.Nombre,
                                           NombreModulo = cl != null ? cl.Nombre : "",
                                           Path = cl != null ? cl.Path : "",
                                           IdMenuPadre = menuPadre == null ? null : menuPadre.Id,
                                           NombreMenuPadre = menuPadre == null ? "" : menuPadre.Nombre,
                                           Grupo = menu.Grupo,
                                           Orden = menu.Orden,
                                           Crear = l != null ? l.Crear : false,
                                           Consultar = l != null ? l.Consultar : false,
                                           Editar = l != null ? l.Editar : false,
                                           Eliminar = l != null ? l.Eliminar : false,
                                       })
                                       .OrderBy(x => x.Grupo)
                                       .ThenBy(x => x.Orden)
                                       .ToArrayAsync();

                return new() { Data = resultado };
            }
            catch (Exception ex)
            {
                return new() { DataError = ex.Message };
            }
        }

        public async Task<Response<bool, string>> Active(ActivarPermisoDto data)
        {
            try
            {
                var permiso = await db.Permisos.FirstOrDefaultAsync(x => x.IdMenu == data.IdMenu && x.IdRol == data.IdRol);
                if (permiso == null)
                {
                    permiso = new Permisos
                    {
                        IdMenu = data.IdMenu,
                        IdRol = data.IdRol,
                        Crear = data.Permiso == "crear",
                        Consultar = data.Permiso == "consultar",
                        Editar = data.Permiso == "editar",
                        Eliminar = data.Permiso == "eliminar"
                    };
                    await db.Permisos.AddAsync(permiso);
                }
                else
                {
                    switch (data.Permiso)
                    {
                        case "crear":
                            permiso.Crear = !permiso.Crear;
                            break;
                        case "consultar":
                            permiso.Consultar = !permiso.Consultar;
                            break;
                        case "editar":
                            permiso.Editar = !permiso.Editar;
                            break;
                        case "eliminar":
                            permiso.Eliminar = !permiso.Eliminar;
                            break;
                        default:
                            return new() { DataError = "Permiso no válido" };
                    }
                    db.Permisos.Update(permiso);
                }
                await db.SaveChangesAsync();
                return new() { Data = true };
            }
            catch (Exception ex)
            {
                return new() { DataError = ex.Message };
            }
        }
    }
}
