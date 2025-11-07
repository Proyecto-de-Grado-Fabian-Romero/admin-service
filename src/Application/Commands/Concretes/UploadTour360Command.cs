using AdminService.Src.Application.Commands.Interfaces;
using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Application.Ports;
using AdminService.Src.Domain.Enums;
using AdminService.Src.Domain.Events;
using AdminService.Src.Domain.Interfaces;

namespace AdminService.Src.Application.Commands.Concretes;

public class UploadTour360Command(
    Guid tourRequestId,
    TourUploadDto tourUpload,
    ITour360RequestRepository repository,
    ITourUploaderAdapter tourUploaderAdapter,
    IEnvironmentServiceAdapter environmentServiceAdapter,
    INotificationsPublisher notificationsPublisher
) : ICommand<bool>
{
    private readonly Guid _tourRequestId = tourRequestId;
    private readonly TourUploadDto _tourUpload = tourUpload;
    private readonly ITour360RequestRepository _repository = repository;
    private readonly ITourUploaderAdapter _tourUploaderAdapter = tourUploaderAdapter;
    private readonly IEnvironmentServiceAdapter _environmentServiceAdapter =
        environmentServiceAdapter;

    private readonly INotificationsPublisher _notificationsPublisher = notificationsPublisher;

    public async Task<bool> ExecuteAsync()
    {
        Console.WriteLine(_tourRequestId);
        var tourRequest =
            await _repository.GetByIdAsync(_tourRequestId)
            ?? throw new Exception("360 Tour Request Not Found");

        if (
            tourRequest.Status != Tour360Status.Pending
            && tourRequest.Status != Tour360Status.Scheduled
        )
        {
            throw new Exception("The Request cannot be Uploaded in Current Status");
        }

        await _tourUploaderAdapter.UploadTourAsync(tourRequest.EnvironmentId, _tourUpload);
        var command = new UpdateTour360StatusCommand(
            _tourRequestId,
            Tour360Status.Completed,
            _repository
        );
        await command.ExecuteAsync();

        tourRequest.Status = Tour360Status.Completed;
        await _repository.UpdateAsync(tourRequest);

        var requestNotification = new SendToUserMessage
        {
            UserPublicId = tourRequest.OwnerId,
            Title = "Tour 360 de Tu Ambiente Añadido",
            Message = "El recorrido 360 de tu ambiente ha sido añadido exitosamente.",
            Type = "Info",
            Channel = "Push",
        };

        _notificationsPublisher.PublishSendToUser(requestNotification);

        return true;
    }
}
