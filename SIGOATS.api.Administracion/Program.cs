#region Librerias

using SIGOATS.api.Api.Extensions;
using SISPRO.TRV.General;
using SISPRO.TRV.Web.MVCCore.StartupExtensions;
using SISPRO.TRV.Web.MVCCore.Helpers;

#endregion


WebApplicationBuilder builder = WebApplicationHelper.CreateCustomBuilder<Program>(args);


ReadConfig.FixLoadAppSettings (builder.Configuration);

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


app.UseCustomConfigure();
app.UseCustomSwagger();

// Require configurar AddHealthChecks() 
// app.CustomMapHealthChecks();

app.Run();
