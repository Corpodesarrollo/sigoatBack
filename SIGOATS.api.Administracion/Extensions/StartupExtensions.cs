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
                options.UseSqlServer(pBuilder.Configuration.GetConnectionString("APP_DBConnectionString"), sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                }));

            pBuilder.Services.AddScoped<IStorageRepo, StorageRepo>();
            pBuilder.Services.AddScoped<IAuthRepo, AuthRepo>();
            pBuilder.Services.AddScoped<AnexosRepo>();
            pBuilder.Services.AddScoped<ArchivosRepo>();
            pBuilder.Services.AddScoped<ContactenosRepo>();
            pBuilder.Services.AddScoped<ExtencionesRepo>();
            pBuilder.Services.AddScoped<ImagenesRepo>();
            pBuilder.Services.AddScoped<NoticiasRepo>();
            pBuilder.Services.AddScoped<NoticiasDetallesRepo>();
            pBuilder.Services.AddScoped<NotificacionesRepo>();
            pBuilder.Services.AddScoped<PaginasRepo>();

            return pBuilder;
        }
    }
}
