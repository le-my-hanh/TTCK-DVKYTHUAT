using Microsoft.AspNetCore.Mvc;

namespace TTCK_DVKYTHUAT.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        [Area("Admin")]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string user, string password, string returnUrl = null)
        {
            if (user.ToLower() == "admin" && password == "123")
            {
                var session = HttpContext.Session;
                session.SetString("user", "admin");

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl); // Điều hướng người dùng đến trang mà họ đã yêu cầu trước đó (nếu có)
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // Trả về trang mặc định nếu không có returnUrl hoặc returnUrl không hợp lệ
                }
            }
            else
            {
                return View();
            }

        }
    }
}
