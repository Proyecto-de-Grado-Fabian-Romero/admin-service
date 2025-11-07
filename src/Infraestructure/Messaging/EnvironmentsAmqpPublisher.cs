using AdminService.Src.Application.Ports;
using AdminService.Src.Domain.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace AdminService.Src.Infraestructure.Messaging;

public sealed class EnvironmentsAmqpPublisher : IEnvironmentsPublisher, IDisposable
{
    private readonly IConnection _conn;
    private readonly IModel _ch;
    private readonly RabbitMqOptions _opt;

    public EnvironmentsAmqpPublisher(IOptions<RabbitMqOptions> opt)
    {
        _opt = opt.Value;
        var f = new ConnectionFactory { Uri = new Uri(_opt.ConnectionString) };
        _conn = f.CreateConnection();
        _ch = _conn.CreateModel();
        _ch.ExchangeDeclare(_opt.Exchange, ExchangeType.Direct, durable: true, autoDelete: false);
        Console.WriteLine("[Publisher] ✅ Connected and exchange declared");
    }

    public void PublishGetEnvironmentDetails(GetEnvironmentDetailsMessage msg)
    {
        var body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
        var props = _ch.CreateBasicProperties();
        props.DeliveryMode = 2;
        props.ReplyTo = "environments.get.details.response";
        props.CorrelationId = msg.CorrelationId;

        Console.WriteLine(
            $"[Publisher] 📤 Publishing 'get.details' with CorrelationId={msg.CorrelationId}"
        );
        Console.WriteLine($"[Publisher] Exchange={_opt.Exchange} Queue='environments.get.details'");

        _ch.BasicPublish(_opt.Exchange, "environments.get.details", props, body);

        Console.WriteLine($"[Publisher] ✅ Message sent successfully");
    }

    public void PublishUpdateDetectedObjects(UpdateDetectedObjectsMessage msg)
    {
        Console.WriteLine(
            $"[Publisher] Publishing UpdateDetectedObjects for env={msg.EnvironmentPublicId}"
        );
        var body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
        var props = _ch.CreateBasicProperties();
        props.DeliveryMode = 2;
        _ch.BasicPublish(_opt.Exchange, "environments.update.objects", props, body);
    }

    public void PublishUploadTour(UploadTourMessage msg)
    {
        Console.WriteLine("[Publisher] Publishing UploadTour");
        Console.WriteLine(
            $"[Publisher] Publishing UploadTour for env={msg.EnvironmentPublicId}"
        );
        var body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
        var props = _ch.CreateBasicProperties();
        props.CorrelationId = msg.CorrelationId;
        props.DeliveryMode = 2;
        _ch.BasicPublish(_opt.Exchange, "environments.tours.upload", props, body);
    }

    public void Dispose()
    {
        _ch?.Dispose();
        _conn?.Dispose();
    }
}
