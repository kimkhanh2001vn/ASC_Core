using ASC.Models.Configuration;
using ASC.Utilities;
using ASC.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASC.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IOptions<ApplicationSettings> _settings;
        public HomeController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }
        //[FromServices] IEmailSender emailSender
        public IActionResult Index()
        {
            //set sesion
            HttpContext.Session.SetSession("Text", _settings.Value);
            //get session
            HttpContext.Session.GetSession<ApplicationSettings>("Test");
            // Usage of IOptions
            ViewBag.Title = _settings.Value.ApplicationTitle;

            //var emailService = this.HttpContext.RequestServices.GetService(typeof(IEmailSender)) as IEmailSender;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
