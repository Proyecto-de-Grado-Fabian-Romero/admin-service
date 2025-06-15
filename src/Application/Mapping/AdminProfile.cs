using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;
using AdminService.Src.Domain.Entities;
using AutoMapper;

namespace AdminService.src.Application.Mapping;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<Tour360Request, Tour360RequestDto>()
            .ForMember(dest => dest.EnvironmentName, opt => opt.Ignore());

        CreateMap<Tour360RequestCreateDto, Tour360Request>();
        CreateMap<Tour360Request, GetTour360RequestsRequest>();

        CreateMap<OwnerDebt, AdminDebtDto>();
        CreateMap<OwnerPayment, AdminPaymentDto>();

        CreateMap<OwnerDebt, AdminDebtDetailDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => string.Empty));
        CreateMap<OwnerPayment, AdminPaymentDetailDto>()
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference));

        CreateMap<CreateOwnerEarningDto, OwnerEarning>()
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.GeneratedAt, opt => opt.MapFrom(src => src.GeneratedAt));
    }
}
