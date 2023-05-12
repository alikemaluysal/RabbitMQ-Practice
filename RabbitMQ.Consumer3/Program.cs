using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };


using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


channel.QueueDeclare(queue: "example-queue", exclusive: false);


EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", false, consumer);
consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.Read();