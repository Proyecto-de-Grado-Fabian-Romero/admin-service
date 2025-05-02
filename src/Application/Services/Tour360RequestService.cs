using AdminService.Src.Application.DTOs;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class Tour360RequestService : ITour360RequestService
{
    private readonly ITour360RequestRepository _repository;
    private readonly IMapper _mapper;

    public Tour360RequestService(ITour360RequestRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<Tour360RequestDto> Items, int TotalItems)> GetAllAsync(GetTour360RequestsRequest request)
    {
        var (entities, totalItems) = await _repository.GetAllAsync(request.Status, request.Page, request.Limit);
        var dtos = _mapper.Map<List<Tour360RequestDto>>(entities);
        return (dtos, totalItems);
    }
}
