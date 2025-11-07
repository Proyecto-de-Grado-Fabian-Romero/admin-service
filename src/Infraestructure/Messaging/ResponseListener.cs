using System.Collections.Concurrent;
using AdminService.Src.Application.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AdminService.Src.Infraestructure.Messaging;

public class ResponseListener : IResponseListener, IDisposable
{
    private readonly ConcurrentDictionary<
        string,
        (Type type, TaskCompletionSource<object> tcs)
    > _responseTasks = new();

    private readonly IConnection _conn;
    private readonly IModel _ch;
    private readonly RabbitMqOptions _opt;

    public ResponseListener(IOptions<RabbitMqOptions> opt)
    {
        _opt = opt.Value;
        var factory = new ConnectionFactory { Uri = new Uri(_opt.ConnectionString) };
        _conn = factory.CreateConnection();
        _ch = _conn.CreateModel();

        _ch.ExchangeDeclare(_opt.Exchange, ExchangeType.Direct, durable: true, autoDelete: false);

        _ch.QueueDeclare(
            "environments.get.details.response",
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        _ch.QueueDeclare(
            "environments.tours.upload.response",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        _ch.QueueBind(
            "environments.get.details.response",
            _opt.Exchange,
            "environments.get.details.response"
        );
        _ch.QueueBind(
            "environments.tours.upload.response",
            _opt.Exchange,
            "environments.tours.upload.response"
        );

        Console.WriteLine("[ResponseListener] ✅ Queues and bindings ready");

        var consumer = new EventingBasicConsumer(_ch);
        consumer.Received += (model, ea) =>
        {
            Console.WriteLine($"[ResponseListener] 📩 Message received on queue {ea.RoutingKey}");

            try
            {
                var correlationId = ea.BasicProperties.CorrelationId;
                Console.WriteLine($"[ResponseListener] CorrelationId = {correlationId}");

                // LOG DETALLADO DEL BODY
                var bodyString = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"[ResponseListener] Raw body length: {ea.Body.Length} bytes");
                Console.WriteLine($"[ResponseListener] Raw body: {bodyString}");

                if (
                    !string.IsNullOrEmpty(correlationId)
                    && _responseTasks.TryRemove(correlationId, out var entry)
                )
                {
                    Console.WriteLine($"[ResponseListener] Found waiting task for {correlationId}");
                    Console.WriteLine($"[ResponseListener] Expected type: {entry.type.Name}");

                    var body = ea.Body.ToArray();

                    // VERIFICA SI EL BODY ESTÁ VACÍO
                    if (body.Length == 0)
                    {
                        Console.WriteLine($"[ResponseListener] ❌ ERROR: Body is empty!");
                        entry.tcs.SetException(
                            new InvalidOperationException("Response body is empty")
                        );
                        _ch.BasicAck(ea.DeliveryTag, false);
                        return;
                    }

                    try
                    {
                        // OPCIÓN 1: Deserialización con opciones
                        var options = new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            NumberHandling = System
                                .Text
                                .Json
                                .Serialization
                                .JsonNumberHandling
                                .AllowReadingFromString,
                        };

                        var responseObj = System.Text.Json.JsonSerializer.Deserialize(
                            body,
                            entry.type,
                            options
                        );

                        if (responseObj == null)
                        {
                            Console.WriteLine(
                                $"[ResponseListener] ❌ ERROR: Deserialized response is NULL"
                            );
                            Console.WriteLine($"[ResponseListener] Body that failed: {bodyString}");
                            entry.tcs.SetException(
                                new InvalidOperationException(
                                    $"Deserialized response was null for type {entry.type.Name}"
                                )
                            );
                        }
                        else
                        {
                            Console.WriteLine(
                                $"[ResponseListener] ✅ Successfully deserialized to type: {responseObj.GetType().Name}"
                            );
                            entry.tcs.SetResult(responseObj);
                        }
                    }
                    catch (System.Text.Json.JsonException jsonEx)
                    {
                        Console.WriteLine(
                            $"[ResponseListener] ❌ JSON Deserialization error: {jsonEx.Message}"
                        );
                        Console.WriteLine($"[ResponseListener] Body that failed: {bodyString}");
                        entry.tcs.SetException(jsonEx);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"[ResponseListener] ❌ General deserialization error: {ex.Message}"
                        );
                        entry.tcs.SetException(ex);
                    }
                }
                else
                {
                    Console.WriteLine(
                        $"[ResponseListener] ⚠️ No task found for correlationId {correlationId}"
                    );
                }

                _ch.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ResponseListener] ❌ Error processing response: {ex.Message}");
                _ch.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        _ch.BasicConsume("environments.get.details.response", false, consumer);
        _ch.BasicConsume("environments.tours.upload.response", false, consumer);

        Console.WriteLine("[ResponseListener] ✅ Consumers started");
    }

    public async Task<T?> WaitForResponse<T>(string correlationId, TimeSpan timeout)
    {
        Console.WriteLine(
            $"[ResponseListener] Waiting for response with correlationId={correlationId} timeout={timeout.TotalSeconds}s"
        );

        var tcs = new TaskCompletionSource<object>();
        _responseTasks[correlationId] = (typeof(T), tcs);

        var timeoutTask = Task.Delay(timeout);
        var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

        if (completedTask == timeoutTask)
        {
            _responseTasks.TryRemove(correlationId, out _);
            Console.WriteLine($"[ResponseListener] ⏰ Timeout for correlationId={correlationId}");
            throw new TimeoutException($"Response timeout after {timeout.TotalSeconds} seconds");
        }

        var result = await tcs.Task;
        Console.WriteLine($"[ResponseListener] ✅ Response received for {correlationId}");
        return result is T typedResult ? typedResult : default;
    }

    public void Dispose()
    {
        _ch?.Close();
        _conn?.Close();
    }

    public void RegisterResponse<T>(string correlationId, T response)
    {
        return;
    }
}
