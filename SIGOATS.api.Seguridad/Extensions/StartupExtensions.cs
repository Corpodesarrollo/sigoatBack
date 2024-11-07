using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.Interfaces;
using SIGOATS.api.Infra;
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
            pBuilder.Services.AddScoped<PermisosRepo>();
            pBuilder.Services.AddScoped<PermisosRolesRepo>();

            return pBuilder;
        }
    }
}
