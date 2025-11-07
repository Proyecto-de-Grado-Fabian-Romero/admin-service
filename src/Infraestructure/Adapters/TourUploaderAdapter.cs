using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Application.Ports;
using AdminService.Src.Domain.Events;

namespace AdminService.Src.Infraestructure.Adapters;

public class TourUploaderAdapter : ITourUploaderAdapter
{
    private readonly IEnvironmentsPublisher _publisher;
    private readonly IResponseListener _responseListener;

    public TourUploaderAdapter(IEnvironmentsPublisher publisher, IResponseListener responseListener)
    {
        _publisher = publisher;
        _responseListener = responseListener;
    }

    public async Task UploadTourAsync(Guid environmentPublicId, TourUploadDto tourUpload)
    {
        var correlationId = Guid.NewGuid().ToString();
        var message = new UploadTourMessage(environmentPublicId, tourUpload, correlationId);

        Console.WriteLine(
            $"[Adapter] 🚀 Uploading tour for environment {environmentPublicId} with correlationId={correlationId}"
        );

        _publisher.PublishUploadTour(message);

        var response = await _responseListener.WaitForResponse<UploadTourMessage>(
            correlationId,
            TimeSpan.FromSeconds(600)
        );

        Console.WriteLine($"[Adapter] ✅ Tour upload response received for {environmentPublicId}");
    }
}
