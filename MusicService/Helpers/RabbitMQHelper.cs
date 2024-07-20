using RabbitMQ.Client;
using System;
using System.Text;

public class RabbitMQHelper : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQHelper()
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqps://bapxqdms:KkoMA2EBEcjnMEaub7vIbnp_Av3mswDk@cow.rmq2.cloudamqp.com/bapxqdms");
        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "comments", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void Publish(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "", routingKey: "comments", basicProperties: null, body: body);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
