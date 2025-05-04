using AdminService.Src.Application.Commands.Concretes;
using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Domain.Entities;
using AutoMapper;

namespace AdminService.src.Application.Mapping;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<Tour360Request, Tour360RequestDto>()
            .ForMember(dest => dest.EnvironmentName, opt => opt.Ignore());

        CreateMap<Tour360Request, Tour360RequestCreateDto>();
        CreateMap<Tour360Request, GetTour360RequestsRequest>();
    }
}
