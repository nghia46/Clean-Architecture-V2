using AutoMapper;
using CleanIsClean.Application.ViewModels;
using CleanIsClean.Domain.Models;

namespace CleanIsClean.API.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserView, User>().ReverseMap();
    }
}
