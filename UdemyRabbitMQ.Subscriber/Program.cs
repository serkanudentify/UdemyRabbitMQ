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

            var randomQueueName = channel.QueueDeclare().QueueName;

            //queueDeclare kullanırsak subs kapanırsa bile kuyruk durur, ama queueBind kullanırsak mesaj gönderildikten sonra silinir.
            channel.QueueBind(randomQueueName, "logs-fanout", string.Empty, null);

            //Tek seferde kaç mesaj to subs
            channel.BasicQos(0, 1, false);

            var subscriber = new EventingBasicConsumer(channel);

            //false:  ise mesajı işledikten sonra silir, true ise mesajı alır almaz siler
            channel.BasicConsume(randomQueueName, false, subscriber);
            Console.WriteLine("We are listening the queue right now!");

            subscriber.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1000);
                Console.WriteLine("A message from queue: " + message);

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
