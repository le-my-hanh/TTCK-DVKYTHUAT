using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;

namespace TTCK_DVKYTHUAT.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();

        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(string user, string password)
        {
            if (user.ToLower() == "admin" && password == "12345")
            {
                var session = HttpContext.Session;
                session.SetString("user", "admin");
                //session["user"] = "admin";

                return RedirectToAction("Index", new { area = "Admin" });
            }
            else
            {
                return View();
            }

        }


    }
    
}
