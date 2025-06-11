using AutoMapper;
using SIGOATS.api.Core.Common;
using SIGOATS.api.Core.DTO;
using SIGOATS.api.Core.Models;

namespace SIGOATS.api.Infra.Common
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Permisos, PermisosDto>();
                cfg.CreateMap<PermisosDto, Permisos>();

                cfg.CreateMap<Notificaciones, NotificacionesDto>();
                cfg.CreateMap<NotificacionesDto, Notificaciones>();

                cfg.CreateMap<Menus, MenusDto>();
                cfg.CreateMap<MenusDto, Menus>();

                cfg.CreateMap<Modulos, ModulosDto>();
                cfg.CreateMap<ModulosDto, Modulos>();

                cfg.CreateMap<Roles, RolesDto>();
                cfg.CreateMap<RolesDto, Roles>();

                cfg.CreateMap<Anexos, AnexosDto>();
                cfg.CreateMap<AnexosDto, Anexos>();

                cfg.CreateMap<Archivos, ArchivosDto>();
                cfg.CreateMap<ArchivosDto, Archivos>();

                cfg.CreateMap<Contactenos, ContactenosDto>();
                cfg.CreateMap<ContactenosDto, Contactenos>();

                cfg.CreateMap<Extenciones, ExtencionesDto>();
                cfg.CreateMap<ExtencionesDto, Extenciones>();

                cfg.CreateMap<Imagenes, ImagenesDto>();
                cfg.CreateMap<ImagenesDto, Imagenes>();

                cfg.CreateMap<Noticias, NoticiasDto>();
                cfg.CreateMap<NoticiasDto, Noticias>();

                cfg.CreateMap<NoticiasDetalles, NoticiasDetallesDto>()
                    .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (TipoItem?)src.Tipo));
                cfg.CreateMap<NoticiasDetallesDto, NoticiasDetalles>()
                    .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => (int?)src.Tipo));

                cfg.CreateMap<Paginas, PaginasDto>();
                cfg.CreateMap<PaginasDto, Paginas>();

                cfg.CreateMap<RedesSociales, RedesSocialesDto>();
                cfg.CreateMap<RedesSocialesDto, RedesSociales>();

                cfg.CreateMap<TiposRedesSociales, TiposRedesSocialesDto>();
                cfg.CreateMap<TiposRedesSocialesDto, TiposRedesSociales>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
