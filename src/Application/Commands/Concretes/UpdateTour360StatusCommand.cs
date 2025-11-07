using AdminService.Src.Application.Commands.Interfaces;
using AdminService.Src.Domain.Enums;
using AdminService.Src.Domain.Interfaces;

namespace AdminService.Src.Application.Commands.Concretes;

public class UpdateTour360StatusCommand(
    Guid requestId,
    Tour360Status newStatus,
    ITour360RequestRepository repository
) : ICommand<bool>
{
    private readonly Guid _requestId = requestId;
    private readonly Tour360Status _newStatus = newStatus;
    private readonly ITour360RequestRepository _repository = repository;

    public async Task<bool> ExecuteAsync()
    {
        var tourRequest = await _repository.GetByIdAsync(_requestId)
                          ?? throw new Exception("360 Tour Request Not Found");

        tourRequest.Status = _newStatus;
        await _repository.UpdateAsync(tourRequest);
        return true;
    }
}
