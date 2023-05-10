using ASP.NET_RabbitMQ_Consumer.Models;

namespace ASP.NET_RabbitMQ_Consumer.Interfaces
{
    public interface IRabbitMQConsumerService
    {
        void Receive();
    }
}
