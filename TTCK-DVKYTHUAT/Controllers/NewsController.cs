using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.Models;

namespace TTCK_DVKYTHUAT.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NewsController> _logger;

        public NewsController(ILogger<NewsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 6;
                var lsNews = _context.News
                    .AsNoTracking().OrderByDescending(X => X.CreatedDate);
                PagedList<News> models = new PagedList<News>(lsNews, pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            var news = _context.News;
            return View(news);
           
        }


        public IActionResult Details(int? Id)
        {
            try
            {
                var news = _context.News.Include(x => x.Categorynew).FirstOrDefault(x => x.NewsId == Id);
                if (news == null)
                {
                    return RedirectToAction("Index");
                }

                var lsnews = _context.News
                .AsNoTracking()
                .Where(x => x.CategorynewId == news.CategorynewId && x.NewsId != Id)
                .Take(3).OrderByDescending(x => x.CreatedDate).ToList();
                ViewBag.tintuc = lsnews;
                return View(news);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

        }

    }
}
