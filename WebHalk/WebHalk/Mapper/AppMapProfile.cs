using AutoMapper;
using WebHalk.Data.Entities;
using WebHalk.Models.Categories;

namespace WebHalk.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<Category, CategoryItemViewModel>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

            CreateMap<Category, CategoryEditViewModel>();
        }
    }
}
