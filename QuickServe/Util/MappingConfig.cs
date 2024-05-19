using AutoMapper;
using QuickServe.Dtos;
using QuickServe.Entities;

namespace QuickServe.Util
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<App, AppCreationDto>();
            CreateMap<AppCreationDto, App>()
                .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
