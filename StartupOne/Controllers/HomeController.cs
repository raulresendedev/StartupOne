using Microsoft.AspNetCore.Mvc;

namespace StartupOne.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
