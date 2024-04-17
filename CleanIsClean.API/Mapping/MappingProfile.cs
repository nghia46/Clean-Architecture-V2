using AutoMapper;
using CleanIsClean.Domain.ViewModels;
using CleanIsClean.Domain.Models;

namespace CleanIsClean.API.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserView, User>().ReverseMap();
        CreateMap<RegisterView, User>().ReverseMap();
        CreateMap<RoleView, Role>().ReverseMap();
    }
}
