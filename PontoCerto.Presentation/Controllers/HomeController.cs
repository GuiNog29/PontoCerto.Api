using Microsoft.AspNetCore.Mvc;

namespace PontoCerto.Presentation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
