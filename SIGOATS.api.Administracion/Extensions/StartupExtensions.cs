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
            // Cargar configuración desde varios orígenes
            pBuilder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{pBuilder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>(optional: true);

            pBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(pBuilder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions =>
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
            pBuilder.Services.AddScoped<TablerosRepo>();
            pBuilder.Services.AddScoped<RedesSocialesRepo>();
            pBuilder.Services.AddScoped<ConfiguracionRepo>();
            pBuilder.Services.AddScoped<EnlaceInteresRepo>();
            pBuilder.Services.AddScoped<FooterInformacionInstitucionalRepo>();
            pBuilder.Services.AddScoped<FooterNormatividadRepo>();
            pBuilder.Services.AddScoped<FooterFaqRepo>();


            return pBuilder;
        }
    }
}
