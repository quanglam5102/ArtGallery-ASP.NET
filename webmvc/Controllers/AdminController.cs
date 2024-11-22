using Microsoft.AspNetCore.Mvc;

namespace webmvc.Controllers
{
    public class AdminController : Controller
    {
        // Admin dashboard page (only accessible by admin)
        public IActionResult Index()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == "Admin")
            {
                return View();
            }

            return RedirectToAction("Signin", "User");
        }
    }
}
