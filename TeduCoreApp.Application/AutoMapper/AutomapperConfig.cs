using AutoMapper;
using System;

namespace TeduCoreApp.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMapping(IServiceProvider provider)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToViewModelMappingProfile());
                cfg.AddProfile(new ViewModelToDomainMappingProfile());
            });
            config.AssertConfigurationIsValid();
            return config;
        }
    }
}