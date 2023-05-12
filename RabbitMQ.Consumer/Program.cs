using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Bağlantı oluşturma
//ConnectionFactory factory = new ConnectionFactory();
//factory.Uri = new("amqps://ywuodjqw:6jSAUzKV5LeINe1h0sDqW2Lp6Ud57yCs@shark.rmq.cloudamqp.com/ywuodjqw");
ConnectionFactory factory = new ConnectionFactory { HostName = "localhost" };


//Bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();


//Queue oluşturma (publisher'daki queue'a bağlanma)
channel.QueueDeclare(queue: "example-queue", exclusive: false);


//Queue'dan mesaj okuma
EventingBasicConsumer consumer = new(channel);
var consumerTag = channel.BasicConsume(queue: "example-queue",autoAck:false,consumer:consumer);
consumer.Received += async (sender, e) =>
{
    //Kuyruğa gelen mesajın işlendiği yerdir
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

    //Message Acknowledgement => Mesajlar başarı ile işlendiyse queue'dan silinir. Normal durumda BasicConsume() fonksiyonundaki autoAck parametresi true verilirse mesaj başarı ile işlensin işlenmesin queue'dan silinir. False verilirse mesajın kuyruktan silinme kararı Message Acknowledgement'a bırakılır. BasicAck() ile bu mesajların eğer başarı ile işlendiyse silinmesi sağlanır. multiple parametresi delivery tag değerine sahip olan mesajla birlikte ondan önceki mesajların da işlendiğini onaylar.
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);


    //Negative Acknowledgement => İşlenmenin başarısız olduğu durumda mesajın tekrardan kuyruğa eklenip eklenmeyeceğini belirler (requeue parametresi)
    channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue:true);


    //consumerTag değerine karşılık gelen "queue'daki tüm mesajlar" reddedilir.
    channel.BasicCancel(consumerTag);

    //deliveryTag değerine karşılık gelen "mesaj" reddedilir.
    channel.BasicReject(deliveryTag:3, requeue:true);
};
  
Console.Read();