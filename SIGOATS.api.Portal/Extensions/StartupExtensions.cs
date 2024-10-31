using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace SIGOATS.api.Api.Extensions
{
	internal static class StartupExtensions
	{
        public static WebApplicationBuilder CustomConfigureServices(this WebApplicationBuilder pBuilder)
        {
            pBuilder.Services.AddDbContext<DbContext>(options =>
                options.UseSqlServer(pBuilder.Configuration.GetConnectionString("DBConnectionString")));

            //pBuilder.Services.AddScoped<IXXX, XXXRepo>();

            //pBuilder.AutomapConfiguration();

            return pBuilder;
        }


        /*private static WebApplicationBuilder AutomapConfiguration(this WebApplicationBuilder pBuilder)
        {
            MapperConfiguration mapConf = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<XXX__Request, XXX>();
                });

            pBuilder.Services.AddSingleton(mapConf.CreateMapper());

            return pBuilder;
        }*/
    }
}
