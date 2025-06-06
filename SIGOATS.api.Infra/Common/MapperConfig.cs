﻿using AutoMapper;
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
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
