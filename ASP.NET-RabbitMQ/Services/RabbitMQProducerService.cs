using ASP.NET_RabbitMQ.Interfaces;
using ASP.NET_RabbitMQ.Models;
using Microsoft.AspNetCore.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ASP.NET_RabbitMQ.Services
{
    public class RabbitMQProducerService :IRabbitMQProducerService
    {
        private readonly IModel _rabbitMQChannel;
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly string _queueName;
        private readonly IConnection _connection;
        public RabbitMQProducerService()
        {
            //create connection and channel
            ConnectionFactory factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672"),
                ClientProvidedName = "Sender App",
            };

            _connection = factory.CreateConnection();
            _rabbitMQChannel = _connection.CreateModel();

            _exchangeName = "MailExchange";
            _routingKey = "mail-routing-key";
            _queueName = "MailQueue";

            //bind the queue 
            _rabbitMQChannel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            _rabbitMQChannel.QueueDeclare(_queueName, false, false, false, null);
            _rabbitMQChannel.QueueBind(_queueName, _exchangeName, _routingKey, null);
        }

        public void Send(Mail mail)
        {
            //sends to the queue
            var mailJson = JsonConvert.SerializeObject(mail);
            var body = Encoding.UTF8.GetBytes(mailJson);

            _rabbitMQChannel.BasicPublish(_exchangeName, _routingKey, null, body);

        }
    }
}
