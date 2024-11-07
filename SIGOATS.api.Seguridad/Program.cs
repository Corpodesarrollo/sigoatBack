#region Librerias

using SIGOATS.api.Api.Extensions;
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

app.UseCors("MyCors");

app.UseCustomConfigure();
app.UseCustomSwagger();

app.Run();
