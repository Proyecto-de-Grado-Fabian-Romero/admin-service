using AdminService.Src.Application.Commands.Concretes;
using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Application.Ports;
using AdminService.Src.Domain.Enums;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Services;

public class Tour360RequestService(
    ITour360RequestRepository repository,
    IMapper mapper,
    IEnvironmentServiceAdapter environmentServiceAdapter,
    ITourUploaderAdapter tourUploadAdapter,
    INotificationsPublisher notificationsPublisher
) : ITour360RequestService
{
    private readonly ITour360RequestRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IEnvironmentServiceAdapter _environmentServiceAdapter =
        environmentServiceAdapter;

    private readonly ITourUploaderAdapter _tourUploadAdapter = tourUploadAdapter;
    private readonly INotificationsPublisher _notificationsPublisher = notificationsPublisher;

    public async Task<Tour360RequestDto> CreateAsync(
        Tour360RequestCreateDto request,
        string authenticatedUserId
    )
    {
        var command = new CreateTour360RequestCommand(
            request,
            authenticatedUserId,
            _repository,
            _mapper,
            _environmentServiceAdapter
        );

        return await command.ExecuteAsync();
    }

    public async Task<(List<Tour360RequestDto> Items, int TotalItems)> GetAllAsync(
        GetTour360RequestsRequest request
    )
    {
        var command = new GetAllTour360RequestsCommand(
            request,
            _repository,
            _mapper,
            _environmentServiceAdapter
        );

        return await command.ExecuteAsync();
    }

    public async Task<bool> Upload360TourAsync(Guid tourRequestId, TourUploadDto uploadDto)
    {
        var command = new UploadTour360Command(
            tourRequestId,
            uploadDto,
            _repository,
            _tourUploadAdapter,
            _environmentServiceAdapter,
            _notificationsPublisher
        );

        return await command.ExecuteAsync();
    }

    public async Task<bool> UpdateStatusAsync(Guid id, Tour360Status newStatus)
    {
        var command = new UpdateTour360StatusCommand(id, newStatus, _repository);
        return await command.ExecuteAsync();
    }

    public async Task<bool> UpdateScheduledDateAsync(Guid id, long scheduledDateUnix)
    {
        var req = await _repository.GetByIdAsync(id);
        if (req is null)
        {
            return false;
        }

        req.ScheduledDate = scheduledDateUnix;
        await _repository.UpdateAsync(req);
        return true;
    }

    public async Task<long?> GetLastRequestDateNonCancelledByEnvironmentAsync(
        Guid environmentPublicId
    )
    {
        var last = await _repository.GetLastNonCancelledByEnvironmentAsync(environmentPublicId);
        return last?.RequestDate;
    }

    public async Task<(List<Tour360RequestDto> Items, int TotalItems)> GetAllByDayAsync(
        GetTour360RequestsRequestByDay request
    )
    {
        var command = new GetAllTour360RequestsByDayCommand(
            request,
            _repository,
            _mapper,
            _environmentServiceAdapter
        );

        return await command.ExecuteAsync();
    }
}
