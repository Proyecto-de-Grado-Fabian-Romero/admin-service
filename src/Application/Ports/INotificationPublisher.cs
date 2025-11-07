using AdminService.Src.Domain.Events;

namespace AdminService.Src.Application.Ports;

public interface INotificationsPublisher
{
    void PublishSendToUser(SendToUserMessage msg);
}
