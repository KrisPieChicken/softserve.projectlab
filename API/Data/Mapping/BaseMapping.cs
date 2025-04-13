using AutoMapper;
using API.Data.Entities;
using softserve.projectlabs.Shared.DTOs;

namespace API.Data.Mapping
{
    public class BaseMapping : Profile
    {
        public BaseMapping()
        {
            CreateMap<BaseEntity, BaseDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            // Si también quieres mapear de DTO a Entity:
            // CreateMap<BaseDto, BaseEntity>();
        }
    }
}
