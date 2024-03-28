using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.ModelsView;

namespace TTCK_DVKYTHUAT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public INotyfService _notifService { get; }
        public AdminAccountsController(ApplicationDbContext context, INotyfService notifService)
        {

            _context = context;
            _notifService = notifService;
        }
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Login() 
        //{
        //    return View();
        //}
      
        //public IActionResult Login(string Username, string Password)
        //{
        //    var account = _context.Accounts.FirstOrDefault(a => a.Username == Username && a.Password == Password);

        //    if (account != null)
        //    {
        //        // Login successful
        //        // Lưu thông tin người dùng vào session
        //        HttpContext.Session.SetInt32("UserId", account.Id);
        //        HttpContext.Session.SetString("Username", account.Username);
        //       // HttpContext.Session.SetString("Roles", account.Roles ?? ""); // Đảm bảo rằng Roles không null

        //        _notifService.Success("Login successful");
        //        return RedirectToAction("Index", "Home"); // Redirect to home page
        //    }
        //    else
        //    {
        //        // Login failed
        //        _notifService.Error("Invalid username or password");
        //        return View(); // Redirect back to login page with error message
        //    }
        //}
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Username == Username && a.Password == Password);

            if (account != null)
            {
                // Login successful
                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetInt32("UserId", account.Id);
                HttpContext.Session.SetString("Username", account.Username);
                // HttpContext.Session.SetString("Roles", account.Roles ?? ""); // Đảm bảo rằng Roles không null

                _notifService.Success("Login successful");
                return RedirectToAction("Index", "Home"); // Redirect to home page
            }
            else
            {
                // Login failed
                _notifService.Error("Invalid username or password");
                return RedirectToAction("Index"); // Redirect back to login page with error message
            }
            //var adminUsername = "Admin";
            //var adminPassword = "123123";

            //var account = _context.Accounts.FirstOrDefault(a => a.Username == adminUsername && a.Password == adminPassword);

            //if (account != null)
            //{
            //    // Admin login successful
            //    // Perform any necessary actions here, such as setting session variables, generating tokens, or redirecting to another page
            //    _notifService.Success("Login successful");
            //    return RedirectToAction("Index", "Home"); // Redirect to admin home page
            //}
            //else
            //{
            //    // Admin login failed
            //    _notifService.Error("Invalid username or password");
            //    return RedirectToAction("Index"); // Redirect back to login page
            //}
        }


        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
