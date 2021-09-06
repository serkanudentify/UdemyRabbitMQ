using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            //Bir factory oluştur ve RabbitMq bağlantı URL'i ver
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://dwfjvqxq:A-d4GvrxPdADiKBvgwAgOrCEfampaYAd@hornet.rmq.cloudamqp.com/dwfjvqxq");

            //Bir bağlantı aç ve channel oluştur.
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // Kuyruk oluştur.
            // queue: kuyruk ismi
            // durable: Memory'de yada Fiziksel olarak tutulsun. false:'de memory'de tutulur, restart atarsan tüm kuyruk boşalır.)
            // exclusive: true ise sadece yukarıda oluşturduğumuz kanal üzerinden iletişim kurulur. Subscriber farklı proje olduğundan false olacak.
            // autoDelete: Kuyruğu dinleyen subscriber ile bağlantı koparsa kuyruk silinir.
            channel.QueueDeclare("hello-queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                //Mesaj yaz ve byte'a çevir.(Byte gönderebiliriz!)
                var messageBody = Encoding.UTF8.GetBytes($"Message {x}");
                //Mesajı kuyruğa gönder.
                channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);
                Console.WriteLine($"Message sent - {x}");
            });
        }
    }
}
