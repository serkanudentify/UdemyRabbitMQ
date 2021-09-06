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
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://dwfjvqxq:A-d4GvrxPdADiKBvgwAgOrCEfampaYAd@hornet.rmq.cloudamqp.com/dwfjvqxq");

            //Bir bağlantı aç ve channel oluştur.
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //Fanout EXCHANGE
            /*
            Fanout Exchange'de kuyruğu publisher oluşturmaz. Kuyruğu dinleyen olmadığı sürece mesajlar boşa gider.
            SendGrid bildirim yapısı gibi. Eğer consumer exchange'i dinlerse subscriber onu dinleyen consumer'ların kuyruğuna mesajı basar.
            Saat başı hava tahmini dağıtan bir api olsun. Sadece bağlı siteler alabilir.
            */
            channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                var messageBody = Encoding.UTF8.GetBytes($"Message {x}");
                channel.BasicPublish("logs-fanout", string.Empty, null, messageBody);
                Console.WriteLine($"Log sent - {x}");
            });
        }
    }
}
