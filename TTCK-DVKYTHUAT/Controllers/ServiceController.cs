using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                var service = _context.Services
                    .Include(x => x.Category)
                    .Include(x=>x.Conments)
                   
                    .FirstOrDefault(x => x.ServiceId == Id);
                if (service == null)
                {
                    return RedirectToAction("Index");
                }

                var lsservices = _context.Services
                .AsNoTracking()
                .Where(x => x.CategoryId == service.CategoryId && x.ServiceId != Id)
                .Take(4).OrderByDescending(x => x.CreatedDate).ToList();
                ViewBag.dichvu = lsservices;
               

                // Tính số sao trung bình
                double averageRating = (double)(service.Conments.Any() ? service.Conments.Average(r => r.Rating) : 0);
                service.AverageRating = Math.Round(averageRating, 1);

                // Lấy danh sách đánh giá cho dịch vụ
                var reviews = await _context.Conments
                    .Include(r => r.Customer)
                    .Where(r => r.ServiceId == Id)
                    .ToListAsync();

                // Thêm danh sách đánh giá vào ViewData
                ViewData["Conments"] = reviews;
                //lấy danh sách các reply
                var replyDictionary = new Dictionary<int, List<Reply>>();

                foreach (var review in reviews)
                {
                    var replies = await _context.Replys
                        .Where(r => r.ConmentId == review.ConmentId)
                        .ToListAsync();

                    replyDictionary.Add(review.ConmentId, replies);
                }

                ViewData["Replies"] = replyDictionary;
                return View(service);
 
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

            

        }
       

        [HttpPost]
        public async Task<IActionResult> CreateReview(int serviceId, int rating, string comment)
        {
            var customerId = HttpContext.Session.GetString("CustomerId");

            if (string.IsNullOrEmpty(customerId))
            {
                // Người dùng chưa đăng nhập, thực hiện xử lý phù hợp, ví dụ: chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Accounts");
            }
           
            // Tạo một đối tượng Review mới
            var review = new Conment
            {
               
                ServiceId = serviceId,
                Rating = rating,
                Message = comment,
                CreateDate = DateTime.Now,
                //EditedAt = DateTime.Now,
                CustomerId = Convert.ToInt32(customerId)
            };

            // Lưu vào cơ sở dữ liệu
            _context.Conments.Add(review);
            await _context.SaveChangesAsync();

            // Quay lại trang chi tiết của nhà hàng hoặc trang nào đó khác
            return RedirectToAction("Details", "Service", new { id = serviceId });
        }

    }
}
