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
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddCustomSwagger();

builder.Services.AddCustomAuthentication(true);


// Registro de los servicios
builder.CustomConfigureServices();

var allowedOrigins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCors",
        policyBuilder =>
        {
            policyBuilder.WithOrigins(allowedOrigins)
                         .AllowAnyHeader()
                         .AllowAnyMethod();
        });
});

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

app.UseCors("MyCors");

app.UseCustomConfigure();
app.UseCustomSwagger();

app.Run();
