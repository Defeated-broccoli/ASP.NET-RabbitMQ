using ASP.NET_RabbitMQ_Consumer.Interfaces;
using ASP.NET_RabbitMQ_Consumer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.NET_RabbitMQ_Consumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRabbitMQConsumerService _rabbitMQConsumerService;

        public HomeController(ILogger<HomeController> logger, IEmailService emailService, IRabbitMQConsumerService rabbitMQConsumerService)
        {
            _logger = logger;
            _rabbitMQConsumerService = rabbitMQConsumerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Check()
        {
            //starts listening on the queue
            _rabbitMQConsumerService.Receive();

            //return confirmation about status
            return View("Listening");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}