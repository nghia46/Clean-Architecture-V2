using AutoMapper;
using CleanIsClean.Application.ViewModels;

namespace CleanIsClean.API.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserView, User>().ReverseMap();
    }
}
