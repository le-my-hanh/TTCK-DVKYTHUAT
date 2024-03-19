using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System.Drawing.Printing;
using System.Security.Claims;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.Models;

namespace TTCK_DVKYTHUAT.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(ILogger<ServiceController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int? page)
        {
            try
            {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = 8;
                var lsServices = _context.Services
                    .AsNoTracking().OrderByDescending(X => X.CreatedDate);
                PagedList<Service> models = new PagedList<Service>(lsServices, pageNumber, pageSize);
                ViewBag.CurrentPage = pageNumber;
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            var servies = _context.Services;
            return View(servies);
        }
        //public async Task<IActionResult> Details(int? Id )
        //{
        //    if(Id == null) return RedirectToAction("Index");
        //    var servicesId = _context.Services.Where(p => p.ServiceId == Id).FirstOrDefault();
        //    return View(servicesId);
        //}

        public async Task<IActionResult> Details(int? Id)
        {
            try
            {
                var service = _context.Services.Include(x => x.Category).FirstOrDefault(x => x.ServiceId == Id);
                if (service == null)
                {
                    return RedirectToAction("Index");
                }

                var lsservices = _context.Services
                .AsNoTracking()
                .Where(x => x.CategoryId == service.CategoryId && x.ServiceId != Id)
                .Take(4).OrderByDescending(x => x.CreatedDate).ToList();
                ViewBag.dichvu = lsservices;
                return View(service);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

            

        }
       


    }
}
