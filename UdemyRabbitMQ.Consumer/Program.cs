using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyRabbitMQ.Consumer
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
            channel.BasicQos(0, 1, false); //tek seferde kaç mesaj
            var subscriber = new EventingBasicConsumer(channel);

            var exchangeTypeName = "header-exchange";
            channel.ExchangeDeclare(exchangeTypeName, durable: true, type: ExchangeType.Headers);

            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add("format", "pdf");
            headers.Add("shape", "a4");
            headers.Add("x-match", "all");
            //headers.Add("x-match", "any");

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "header-exchange", string.Empty, headers);

            //false:  ise mesajı işledikten sonra silir, true ise mesajı alır almaz siler
            channel.BasicConsume(queueName, false, subscriber);
            Console.WriteLine("We are listening the queue right now!");

            subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine("A message from queue: " + message);

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
