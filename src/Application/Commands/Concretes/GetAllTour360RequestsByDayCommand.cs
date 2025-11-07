using AdminService.Src.Application.Commands.Interfaces;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Interfaces;
using AutoMapper;

namespace AdminService.Src.Application.Commands.Concretes;

public class GetAllTour360RequestsByDayCommand(
    GetTour360RequestsRequestByDay request,
    ITour360RequestRepository repository,
    IMapper mapper,
    IEnvironmentServiceAdapter environmentServiceAdapter
) : ICommand<(List<Tour360RequestDto> Items, int TotalItems)>
{
    private readonly GetTour360RequestsRequestByDay _request = request;
    private readonly ITour360RequestRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IEnvironmentServiceAdapter _environmentServiceAdapter =
        environmentServiceAdapter;

    public async Task<(List<Tour360RequestDto> Items, int TotalItems)> ExecuteAsync()
    {
        if (!_request.ScheduledDayTimestamp.HasValue)
        {
            throw new System.ArgumentNullException(
                nameof(_request.ScheduledDayTimestamp),
                "ScheduledDayTimestamp is required."
            );
        }

        var requestedDay = DateTimeOffset
            .FromUnixTimeSeconds(_request.ScheduledDayTimestamp.Value)
            .ToOffset(TimeSpan.FromHours(-4))
            .Date;

        var startOfDayTimestamp = new DateTimeOffset(requestedDay).ToUnixTimeSeconds();
        var endOfDayTimestamp = new DateTimeOffset(requestedDay.AddDays(1)).ToUnixTimeSeconds();

        var (entities, totalItems) = await _repository.GetAllByScheduledDayAsync(
            _request.Status,
            startOfDayTimestamp,
            endOfDayTimestamp,
            _request.Page,
            _request.Limit
        );

        var dtos = new List<Tour360RequestDto>();

        foreach (var entity in entities)
        {
            var dto = _mapper.Map<Tour360RequestDto>(entity);
            var environmentInfo = await _environmentServiceAdapter.GetEnvironmentDetailsAsync(
                entity.EnvironmentId
            );
            dto.EnvironmentName = environmentInfo?.Title ?? "Nombre no disponible";
            dtos.Add(dto);
        }

        return (dtos, totalItems);
    }
}
