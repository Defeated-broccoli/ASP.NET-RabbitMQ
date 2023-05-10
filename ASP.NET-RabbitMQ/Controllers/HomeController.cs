using ASP.NET_RabbitMQ.Interfaces;
using ASP.NET_RabbitMQ.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP.NET_RabbitMQ.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRabbitMQProducerService _rabbitMQProducerService;

        public HomeController(ILogger<HomeController> logger, IRabbitMQProducerService rabbitMQProducerService)
        {
            _logger = logger;
            //dependency injection of the rabbit service (Singleton)
            _rabbitMQProducerService = rabbitMQProducerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Mail mail)
        {
            //check model, return View if invalid
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Send");
                return View(mail);
            }

            //send to the queue
            _rabbitMQProducerService.Send(mail);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}