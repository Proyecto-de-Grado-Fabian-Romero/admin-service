using AdminService.src.Application.DTOs.Response;
using AdminService.Src.Application.DTOs.Response;
using AdminService.Src.Application.Interfaces;
using AdminService.Src.Application.Ports;
using AdminService.Src.Domain.Events;

namespace AdminService.Src.Infraestructure.Adapters;

public class EnvironmentServiceAdapter : IEnvironmentServiceAdapter
{
    private readonly IEnvironmentsPublisher _publisher;
    private readonly IResponseListener _responseListener;

    public EnvironmentServiceAdapter(
        IEnvironmentsPublisher publisher,
        IResponseListener responseListener
    )
    {
        _publisher = publisher;
        _responseListener = responseListener;
    }

    public async Task<EnvironmentDetailsResponse?> GetEnvironmentDetailsAsync(
        Guid environmentPublicId
    )
    {
        var correlationId = Guid.NewGuid().ToString();
        Console.WriteLine(
            $"[Adapter] 🔍 Requesting environment details for {environmentPublicId} with correlationId={correlationId}"
        );

        var message = new GetEnvironmentDetailsMessage(environmentPublicId, correlationId);
        _publisher.PublishGetEnvironmentDetails(message);

        Console.WriteLine($"[Adapter] 🕓 Waiting for response...");
        var response = await _responseListener.WaitForResponse<EnvironmentDetailsResponse>(
            correlationId,
            TimeSpan.FromSeconds(30)
        );

        Console.WriteLine($"[Adapter] ✅ Response received for {environmentPublicId}");
        return response;
    }

    public async Task UpdateDetectedObjectsAsync(
        Guid environmentPublicId,
        Dictionary<string, int> detectedObjects
    )
    {
        Console.WriteLine($"[Adapter] 🛰 Updating detected objects for {environmentPublicId}");
        var message = new UpdateDetectedObjectsMessage(environmentPublicId, detectedObjects);
        _publisher.PublishUpdateDetectedObjects(message);
        await Task.CompletedTask;
    }
}
