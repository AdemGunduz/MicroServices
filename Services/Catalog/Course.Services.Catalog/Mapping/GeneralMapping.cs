using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;

namespace Course.Services.Catalog.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
           CreateMap<Coursec,CourseDto>().ReverseMap();
           CreateMap<Category,CategoryDto>().ReverseMap();
           CreateMap<Feature,FeatureDto>().ReverseMap();

           CreateMap<Coursec,CourseCreateDto>().ReverseMap();
           CreateMap<Coursec,CourseUpdateDto>().ReverseMap();
        }
    }
}
