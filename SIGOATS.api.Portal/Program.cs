#region Librerias

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SIGOATS.api.Api.Extensions;
using SISPRO.TRV.General;
using SISPRO.TRV.Web.MVCCore.Helpers;
using SISPRO.TRV.Web.MVCCore.StartupExtensions;
using System.Text.Json;

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

// Registro de verificaciones Health
// Ref: https://medium.com/@jeslurrahman/implementing-health-checks-in-net-8-c3ba10af83c3
// builder.Services.AddHealthChecks();

// Registro de los servicios
builder.CustomConfigureServices();


WebApplication app = builder.Build();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = "El servicio esta disponible"
        });
        await context.Response.WriteAsync(result);
    }
});

app.UseCustomConfigure();
app.UseCustomSwagger();

// Require configurar AddHealthChecks() 
// app.CustomMapHealthChecks();

app.Run();
