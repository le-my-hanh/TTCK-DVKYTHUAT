using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.Models;

namespace TTCK_DVKYTHUAT.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public async Task<IActionResult> Index(int? categoreId, int? page )
        {

            if (categoreId == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == categoreId);
            if (category == null)
            {
                return NotFound();
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            // Lấy danh sách nhà hàng thuộc danh mục và thực hiện phân trang
            var services = _context.Services
                .Where(r => r.CategoryId == categoreId)
                .ToPagedList(pageNumber, pageSize);

            // Truyền dữ liệu vào view
            ViewData["Category"] = category;
            ViewData["Services"] = services;
            // Lưu danh sách hình ảnh vào ViewData
            return View(category);
            
        }
        
    }
}
