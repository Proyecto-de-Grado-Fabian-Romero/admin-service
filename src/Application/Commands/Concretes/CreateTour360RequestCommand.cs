using AdminService.Src.Application.Commands.Interfaces;
using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Entities;
using AdminService.Src.Domain.Enums;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Commands.Concretes;

public class CreateTour360RequestCommand(
    Tour360RequestCreateDto request,
    string authenticatedUserId,
    ITour360RequestRepository repository,
    IMapper mapper,
    IEnvironmentServiceAdapter environmentServiceAdapter) : ICommand<Tour360RequestDto>
{
    private readonly Tour360RequestCreateDto _request = request;
    private readonly string _authenticatedUserId = authenticatedUserId;
    private readonly ITour360RequestRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IEnvironmentServiceAdapter _environmentServiceAdapter = environmentServiceAdapter;

    public async Task<Tour360RequestDto> ExecuteAsync()
    {
        var environment = await _environmentServiceAdapter.GetEnvironmentDetailsAsync(_request.EnvironmentId)
                          ?? throw new Exception("Ambiente no encontrado.");

        if (environment.OwnerId.ToString() != _authenticatedUserId)
        {
            throw new UnauthorizedAccessException("No tienes permisos sobre este ambiente.");
        }

        var entity = new Tour360Request
        {
            EnvironmentId = _request.EnvironmentId,
            OwnerId = Guid.Parse(_authenticatedUserId),
            RequestDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Status = Tour360Status.Pending,
        };

        var createdEntity = await _repository.AddAsync(entity);

        var dto = _mapper.Map<Tour360RequestDto>(createdEntity);
        dto.EnvironmentName = environment.Title ?? "Nombre no disponible";

        return dto;
    }
}
