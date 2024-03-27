using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TTCK_DVKYTHUAT.Data;

namespace TTCK_DVKYTHUAT.Controllers
{
    public class CategoryNewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryNewsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? categoreNewsId, int? page)
        {
            if (categoreNewsId == null || _context.CategoryNews == null)
            {
                return NotFound();
            }

            var category = await _context.CategoryNews
                .FirstOrDefaultAsync(m => m.CategorynewId == categoreNewsId);
            if (category == null)
            {
                return NotFound();
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            // Lấy danh sách nhà hàng thuộc danh mục và thực hiện phân trang
            var news = _context.News
                .Where(r => r.CategorynewId == categoreNewsId)
                .ToPagedList(pageNumber, pageSize);

            // Truyền dữ liệu vào view
            ViewData["CategoryNews"] = category;
            ViewData["News"] = news;
            // Lưu danh sách hình ảnh vào ViewData
            return View(category);
        }
    }
}
