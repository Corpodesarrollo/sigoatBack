using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;
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
                options.UseSqlServer(pBuilder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                }));

            pBuilder.Services.AddScoped<IAuthRepo, AuthRepo>();
            pBuilder.Services.AddScoped<MenusRepo>();
            pBuilder.Services.AddScoped<MenusPortalRepo>();
            pBuilder.Services.AddScoped<ModulosRepo>();
            pBuilder.Services.AddScoped<RolesRepo>();
            pBuilder.Services.AddScoped<UsersRepo>();
            pBuilder.Services.AddScoped<PermisosRepo>();
            pBuilder.Services.AddScoped<SeguridadRepo>();
            pBuilder.Services.AddScoped<AesEncryptionRepo>();
            pBuilder.Services.AddScoped<EmailManagerRepo>();
            pBuilder.Services.AddScoped<CodigosSeguridadRepo>();
            pBuilder.Services.AddScoped<VariablesRepo>();
            pBuilder.Services.AddScoped<ConfigSMTPRepo>();
            pBuilder.Services.AddScoped<EmailsRepo>();
            pBuilder.Services.AddScoped<HelpersRepo>();

            pBuilder.Services.AddIdentity<Usuarios, Roles>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            pBuilder.Services.Configure<EncryptionSettingsDto>(pBuilder.Configuration.GetSection("EncryptionSettings"));

            return pBuilder;
        }
    }
}
