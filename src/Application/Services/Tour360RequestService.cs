using AdminService.Src.Application.Commands.Concretes;
using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class Tour360RequestService(
    ITour360RequestRepository repository,
    IMapper mapper,
    IEnvironmentServiceAdapter environmentServiceAdapter,
    ITourUploaderAdapter tourUploadAdapter
    ) : ITour360RequestService
{
    private readonly ITour360RequestRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IEnvironmentServiceAdapter _environmentServiceAdapter = environmentServiceAdapter;
    private readonly ITourUploaderAdapter _tourUploadAdapter = tourUploadAdapter;

    public async Task<Tour360RequestDto> CreateAsync(Tour360RequestCreateDto request, string authenticatedUserId)
    {
        var command = new CreateTour360RequestCommand(
            request,
            authenticatedUserId,
            _repository,
            _mapper,
            _environmentServiceAdapter);

        return await command.ExecuteAsync();
    }

    public async Task<(List<Tour360RequestDto> Items, int TotalItems)> GetAllAsync(GetTour360RequestsRequest request)
    {
        var command = new GetAllTour360RequestsCommand(
            request,
            _repository,
            _mapper,
            _environmentServiceAdapter);

        return await command.ExecuteAsync();
    }

    public async Task<bool> Upload360TourAsync(Guid tourRequestId, TourUploadDto uploadDto)
    {
        var command = new UploadTour360Command(
            tourRequestId,
            uploadDto,
            _repository,
            _tourUploadAdapter);

        return await command.ExecuteAsync();
    }
}
