using ASP.NET_RabbitMQ.Models;

namespace ASP.NET_RabbitMQ.Interfaces
{
    public interface IRabbitMQProducerService
    {
        void Send(Mail mail);
    }
}
