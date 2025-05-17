using AdminService.Src.Application.Commands.Interfaces;
using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Domain.Enums;
using AdminService.Src.Domain.Interfaces;

namespace AdminService.Src.Application.Commands.Concretes;

public class UploadTour360Command(
    Guid tourRequestId,
    TourUploadDto tourUpload,
    ITour360RequestRepository repository,
    ITourUploaderAdapter tourUploaderAdapter,
    IObjectDetectionAdapter objectDetectionAdapter,
    IEnvironmentServiceAdapter environmentServiceAdapter
) : ICommand<bool>
{
    private readonly Guid _tourRequestId = tourRequestId;
    private readonly TourUploadDto _tourUpload = tourUpload;
    private readonly ITour360RequestRepository _repository = repository;
    private readonly ITourUploaderAdapter _tourUploaderAdapter = tourUploaderAdapter;
    private readonly IObjectDetectionAdapter _objectDetectionAdapter = objectDetectionAdapter;
    private readonly IEnvironmentServiceAdapter _environmentServiceAdapter = environmentServiceAdapter;

    public async Task<bool> ExecuteAsync()
    {
        var tourRequest = await _repository.GetByIdAsync(_tourRequestId)
                          ?? throw new Exception("360 Tour Request Not Found");

        if (tourRequest.Status != Tour360Status.Pending &&
            tourRequest.Status != Tour360Status.Scheduled)
        {
            throw new Exception("The Request cannot be Uploaded in Current Status");
        }

        await _tourUploaderAdapter.UploadTourAsync(tourRequest.EnvironmentId, _tourUpload);

        tourRequest.Status = Tour360Status.Completed;
        await _repository.UpdateAsync(tourRequest);

        return true;
    }
}
