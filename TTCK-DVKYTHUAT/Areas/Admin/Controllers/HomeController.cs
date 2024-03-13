using Microsoft.AspNetCore.Mvc;

namespace TTCK_DVKYTHUAT.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
