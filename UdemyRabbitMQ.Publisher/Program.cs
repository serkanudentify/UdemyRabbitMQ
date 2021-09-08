using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://dwfjvqxq:A-d4GvrxPdADiKBvgwAgOrCEfampaYAd@hornet.rmq.cloudamqp.com/dwfjvqxq");

            //Bir bağlantı aç ve channel oluştur.
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var exchangeTypeName = "logs-topic";
            channel.ExchangeDeclare(exchangeTypeName, durable: true, type: ExchangeType.Topic);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                Random rnd = new Random();
                LogNames log1 = (LogNames)rnd.Next(1, 5);
                LogNames log2 = (LogNames)rnd.Next(1, 5);
                LogNames log3 = (LogNames)rnd.Next(1, 5);
                var routeKey = $"{log1}.{log2}.{log3}";

                var message = $"{routeKey}.{x}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                
                channel.BasicPublish(exchangeTypeName, routeKey, null, messageBody);
                Console.WriteLine($"Log sent - {x}");
            });
        }
    }
}
