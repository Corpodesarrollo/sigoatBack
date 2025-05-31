using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Infra;
using SIGOATS.api.Infra.Interfaces;
using SIGOATS.api.Infra.Repositorios;


namespace SIGOATS.api.Api.Extensions
{
    internal static class StartupExtensions
    {
        public static WebApplicationBuilder CustomConfigureServices(this WebApplicationBuilder pBuilder)
        {
            pBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(pBuilder.Configuration.GetConnectionString("APP_DBConnectionString")));

            pBuilder.Services.AddScoped<IAuthRepo, AuthRepo>();
            pBuilder.Services.AddScoped<MenusRepo>();
            pBuilder.Services.AddScoped<ModulosRepo>();
            pBuilder.Services.AddScoped<RolesRepo>();
            pBuilder.Services.AddScoped<PermisosRepo>();

            return pBuilder;
        }
    }
}
