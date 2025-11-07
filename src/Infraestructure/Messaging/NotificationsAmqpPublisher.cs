namespace AdminService.Src.Infraestructure.Messaging;

using AdminService.Src.Application.Ports;
using AdminService.Src.Domain.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

public sealed class NotificationsAmqpPublisher : INotificationsPublisher, IDisposable
{
    private readonly IConnection _conn;
    private readonly IModel _ch;
    private readonly RabbitMqOptions _opt;

    public NotificationsAmqpPublisher(IOptions<RabbitMqOptions> opt)
    {
        _opt = opt.Value;
        var f = new ConnectionFactory { Uri = new Uri(_opt.ConnectionString) };
        _conn = f.CreateConnection();
        _ch = _conn.CreateModel();
        _ch.ExchangeDeclare(_opt.Exchange, ExchangeType.Direct, durable: true, autoDelete: false);
    }

    public void PublishSendToUser(SendToUserMessage msg)
    {
        var body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
        var props = _ch.CreateBasicProperties();
        props.DeliveryMode = 2;
        _ch.BasicPublish(_opt.Exchange, "user.send", props, body);
        Console.WriteLine("Published notification to user");
    }

    public void Dispose()
    {
        _ch?.Dispose();
        _conn?.Dispose();
    }
}
