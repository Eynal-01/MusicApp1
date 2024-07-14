using RabbitMQ.Client;
using System.Text;

public class RabbitMQHelper
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQHelper()
    {
        //var factory = new ConnectionFactory() { HostName = "amqps://nfazpmyg:qr95LfbgexB79kOgS3wfrJrBZ4Yv0_IB@cow.rmq2.cloudamqp.com/nfazpmyg" };
        //_connection = factory.CreateConnection();
        //_channel = _connection.CreateModel();
        //_channel.QueueDeclare(queue: "comments", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqps://nfazpmyg:qr95LfbgexB79kOgS3wfrJrBZ4Yv0_IB@cow.rmq2.cloudamqp.com/nfazpmyg");

        using var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

        channel.QueueDeclare("", true, false, false);
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
