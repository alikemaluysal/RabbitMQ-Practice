using RabbitMQ.Client;
using System.Text;

//Bağlantı oluşturma
//ConnectionFactory factory = new ConnectionFactory();
//factory.Uri = new("amqps://ywuodjqw:6jSAUzKV5LeINe1h0sDqW2Lp6Ud57yCs@shark.rmq.cloudamqp.com/ywuodjqw");
ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };



//Bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


//Queue oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable:true);
//durable:true => Message Durability


//Queue'ya mesaj gönderme
//RabbitMQ kuryuğa atacağı mesajları byte türünden kabul etmektedir. Haliyle mesajları bizim byte dönüşmemiz gerekmektedir

//byte[] message= Encoding.UTF8.GetBytes("Merhaba");
//channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);

//for (int i = 0; i < 200; i++)
//{
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
//    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
//    Thread.Sleep(100);
//    Console.WriteLine(i + ". mesaj gönderildi.");
//}

//Message Durability - 2
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent= true;


int i = 0;
string isCont = "devam";
while(isCont == "devam")
{
    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message, basicProperties:properties);
    Thread.Sleep(100);
    Console.WriteLine(i + ". mesaj gönderildi.");
    i++;
    if (i % 100 == 0) {
        isCont = Console.ReadLine().ToString();
    }
}

Console.Read();