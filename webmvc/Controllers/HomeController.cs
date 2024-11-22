using Microsoft.AspNetCore.Mvc;

namespace webmvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            ViewData["Role"] = role;
            return View();
        }
    }
}
