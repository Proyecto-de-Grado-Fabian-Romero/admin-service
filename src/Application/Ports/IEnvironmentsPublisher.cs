using AdminService.Src.Domain.Events;

namespace AdminService.Src.Application.Ports;

public interface IEnvironmentsPublisher
{
    void PublishGetEnvironmentDetails(GetEnvironmentDetailsMessage msg);

    void PublishUpdateDetectedObjects(UpdateDetectedObjectsMessage msg);

    void PublishUploadTour(UploadTourMessage msg);
}
