using ASP.NET_RabbitMQ_Consumer.Interfaces;
using ASP.NET_RabbitMQ_Consumer.Models;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ASP.NET_RabbitMQ_Consumer.Services
{
    public class RabbitMQConsumerService :IRabbitMQConsumerService
    {
        private readonly IModel _rabbitMQChannel;
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IEmailService _emailService;

        public RabbitMQConsumerService(IEmailService emailService)
        {
            //create connection and channel
            ConnectionFactory factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672"),
                ClientProvidedName = "Receiver App",
            };

            _connection = factory.CreateConnection();
            _rabbitMQChannel = _connection.CreateModel();

            _queueName = "MailQueue";

            _rabbitMQChannel.QueueDeclare(_queueName, false, false, false, null);
            _emailService = emailService;
        }

        public void Receive()
        {
            //create consumer
            var consumer = new EventingBasicConsumer(_rabbitMQChannel);

            //listen on the queue
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var mail = JsonConvert.DeserializeObject<Mail>(message);

                //send email
                _emailService.SendEmailAsync(mail);

                _rabbitMQChannel.BasicAck(ea.DeliveryTag, false);
            };

            string consumerTag = _rabbitMQChannel.BasicConsume(_queueName, false, consumer);
        }
    }
}
