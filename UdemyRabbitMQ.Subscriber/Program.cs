using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.Subscriber
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

            //kuyruğun olduğundan eminsen kaldır
            //channel.QueueDeclare("hello-queue", true, false, false);

            //tek seferde kaç mesaj to subs
            channel.BasicQos(0, 1, false);

            var subscriber = new EventingBasicConsumer(channel);

            //false:  ise mesajı işledikten sonra silir, true ise mesajı alır almaz siler
            channel.BasicConsume("hello-queue", false, subscriber);

            subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1000);
                Console.WriteLine("Given message: " + message);

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
