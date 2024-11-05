#region Librerias

using SIGOATS.api.Api.Extensions;
using SIGOATS.api.Core.Interfaces;
using SIGOATS.api.Infra.Repositorios;
using SISPRO.TRV.General;
using SISPRO.TRV.Web.MVCCore.Helpers;
using SISPRO.TRV.Web.MVCCore.StartupExtensions;

#endregion


WebApplicationBuilder builder = WebApplicationHelper.CreateCustomBuilder<Program>(args);


ReadConfig.FixLoadAppSettings(builder.Configuration);

builder.Services.AddCustomConfigureServicesPreviousMvc();
builder
    .Services
    .AddCustomMvcControllers()
    .AddJsonOptions();
//.AddFluentValidation<ValidarSolicitante_RequestValidator>();

builder.Services.AddCustomSwagger();
builder.Services.AddCustomAuthentication(true);

// Registro de los servicios
builder.CustomConfigureServices();

builder.Services.AddScoped<IAuthRepo, AuthRepo>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://192.168.110.11:8140", "http://localhost:4200", "https://localhost:4200", "https://secani-cbabfpddahe6ayg9.eastus-01.azurewebsites.net")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});

WebApplication app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.UseCustomConfigure();
app.UseCustomSwagger();

app.Run();
