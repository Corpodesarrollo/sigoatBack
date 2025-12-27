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
                cfg.CreateMap<Configuracion, ConfiguracionDto>();
                cfg.CreateMap<ConfiguracionDto, Configuracion>();

                cfg.CreateMap<Permisos, PermisosDto>();
                cfg.CreateMap<PermisosDto, Permisos>();

                cfg.CreateMap<Notificaciones, NotificacionesDto>()
                    .ForMember(dest => dest.TipoEvento, opt => opt.MapFrom(src => (TipoEvento?)src.TipoEvento))
                    .ForMember(dest => dest.Audiencia, opt => opt.MapFrom(src => (Audiencia?)src.Audiencia));
                cfg.CreateMap<NotificacionesDto, Notificaciones>()
                    .ForMember(dest => dest.TipoEvento, opt => opt.MapFrom(src => (int?)src.TipoEvento))
                    .ForMember(dest => dest.Audiencia, opt => opt.MapFrom(src => (int?)src.Audiencia));

                cfg.CreateMap<Menus, MenusDto>();
                cfg.CreateMap<MenusDto, Menus>();

                cfg.CreateMap<MenusPortal, MenusPortalDto>();
                cfg.CreateMap<MenusPortalDto, MenusPortal>();

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

                cfg.CreateMap<Tableros, TablerosDto>();
                cfg.CreateMap<TablerosDto, Tableros>();

                cfg.CreateMap<NotificacionesLeidas, NotificacionesLeidasDto>();
                cfg.CreateMap<NotificacionesLeidasDto, NotificacionesLeidas>();

                cfg.CreateMap<Usuarios, UserDto>();
                cfg.CreateMap<UserDto, Usuarios>();

                cfg.CreateMap<EnlacesInteres, EnlacesInteresDto>();
                cfg.CreateMap<EnlacesInteresDto, EnlacesInteres>();

                cfg.CreateMap<FooterInformacionInstitucional, FooterInformacionInstitucionalDto>();
                cfg.CreateMap<FooterInformacionInstitucionalDto, FooterInformacionInstitucional>();

                cfg.CreateMap<FooterNormatividad, FooterNormatividadDto>();
                cfg.CreateMap<FooterNormatividadDto, FooterNormatividad>();

                cfg.CreateMap<FooterFaq, FooterFaqDto>();
                cfg.CreateMap<FooterFaqDto, FooterFaq>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
