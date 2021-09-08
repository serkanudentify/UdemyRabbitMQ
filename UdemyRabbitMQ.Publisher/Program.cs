using RabbitMQ.Client;
using System;
using System.Collections.Generic;
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

            var exchangeTypeName = "header-exchange";
            channel.ExchangeDeclare(exchangeTypeName, durable: true, type: ExchangeType.Headers);

            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("format", "pdf");
            headers.Add("shape", "a4d");

            var properties = channel.CreateBasicProperties();
            properties.Headers = headers;

            channel.BasicPublish(exchangeTypeName, string.Empty, properties, Encoding.UTF8.GetBytes("Header Message"));

            Console.WriteLine("Message Sent!");
            Console.ReadLine();
        }
    }
}
