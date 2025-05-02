using AdminService.Src.Application.DTOs;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class Tour360RequestService(
    ITour360RequestRepository repository,
    IMapper mapper,
    IEnvironmentServiceAdapter environmentService) : ITour360RequestService
{
    private readonly ITour360RequestRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IEnvironmentServiceAdapter _environmentService = environmentService;

    public async Task<(List<Tour360RequestDto> Items, int TotalItems)> GetAllAsync(GetTour360RequestsRequest request)
    {
        var (entities, totalItems) = await _repository.GetAllAsync(request.Status, request.Page, request.Limit);

        var dtos = new List<Tour360RequestDto>();

        foreach (var entity in entities)
        {
            var dto = _mapper.Map<Tour360RequestDto>(entity);

            var environmentName = await _environmentService.GetEnvironmentNameAsync(entity.EnvironmentId);
            dto.EnvironmentName = environmentName ?? "Nombre no disponible";

            dtos.Add(dto);
        }

        return (dtos, totalItems);
    }
}
