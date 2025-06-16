using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;
using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Enums;
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

        CreateMap<CreateOwnerEarningDto, OwnerEarning>()
             .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
             .ForMember(dest => dest.GeneratedAt, opt => opt.MapFrom(src => src.GeneratedAt));

        CreateMap<OwnerEarning, OwnerDebt>()
        .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Amount))
        .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
        .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));

        CreateMap<OwnerDebt, OwnerPayment>()
            .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => PaymentMethod.BankTransfer))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
    }
}
