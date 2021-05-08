using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Project.Web.Models;

namespace Project.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(nameof(Error))]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
