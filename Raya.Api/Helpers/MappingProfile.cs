using AutoMapper;
using Raya.Api.Dtos;
using Raya.Core.Entities;

namespace Raya.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductAddDto>().ReverseMap();
        }
    }
}
