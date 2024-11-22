using Microsoft.AspNetCore.Mvc;
using System.Text;
using webmvc.Models;
using System.Security.Cryptography;

namespace webmvc.Controllers
{
    public class UserController : Controller
    {
        private readonly ArtContext _context;

        public UserController(ArtContext context)
        {
            _context = context;
        }

        // GET: Signup
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(AuthSignupModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email already exists.");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password),
                    Role = model.Role ?? "User" // Default to 'User' if no role is provided
                };
                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Signin");
            }

            return View(model);
        }

        // GET: Signin
        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signin(AuthSigninModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(model.Password);

                var user = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == hashedPassword);

                if (user != null)
                {
                    // Store user data in Session using HttpContext.Session
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);

                    // Redirect to home page after successful login
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid email or password.");
            }

            return View(model);
        }

        public IActionResult AdminDashboard()
        {
            // Check if user is logged in and has admin role
            var userId = HttpContext.Session.GetString("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (!string.IsNullOrEmpty(userId) && role == "Admin")
            {
                return View();
            }

            return RedirectToAction("Signin", "User");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Signin", "User");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
