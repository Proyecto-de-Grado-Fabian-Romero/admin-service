using AdminService.Src.Application.DTOs;
using AdminService.Src.Domain.Entities;
using AutoMapper;

namespace AdminService.src.Application.Mapping;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<Tour360Request, Tour360RequestDto>()
            .ForMember(dest => dest.EnvironmentName, opt => opt.Ignore());
    }
}
