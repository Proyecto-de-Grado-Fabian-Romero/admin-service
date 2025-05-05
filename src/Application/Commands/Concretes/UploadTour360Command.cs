using AdminService.Src.Application.Commands.Interfaces;
using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Interfaces;

namespace AdminService.Src.Application.Commands.Concretes;

public class UploadTour360Command : ICommand<bool>
{
    private readonly Guid _tourRequestId;
    private readonly TourUploadDto _tourUpload;
    private readonly ITour360RequestRepository _repository;
    private readonly ITourUploaderAdapter _tourUploaderAdapter;

    public UploadTour360Command(
        Guid tourRequestId,
        TourUploadDto tourUpload,
        ITour360RequestRepository repository,
        ITourUploaderAdapter tourUploaderAdapter)
    {
        _tourRequestId = tourRequestId;
        _tourUpload = tourUpload;
        _repository = repository;
        _tourUploaderAdapter = tourUploaderAdapter;
    }

    public async Task<bool> ExecuteAsync()
    {
        var tourRequest = await _repository.GetByIdAsync(_tourRequestId)
                          ?? throw new Exception("360 Tour Request Not Found");

        if (tourRequest.Status != Domain.Enums.Tour360Status.Pending &&
            tourRequest.Status != Domain.Enums.Tour360Status.Scheduled)
        {
            throw new Exception("The Request cannot be Uploaded in Current Status");
        }

        await _tourUploaderAdapter.UploadTourAsync(tourRequest.EnvironmentId, _tourUpload);

        tourRequest.Status = Domain.Enums.Tour360Status.Completed;
        await _repository.UpdateAsync(tourRequest);

        return true;
    }
}
