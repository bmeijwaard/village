using Microsoft.AspNetCore.Mvc;

namespace Village.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
