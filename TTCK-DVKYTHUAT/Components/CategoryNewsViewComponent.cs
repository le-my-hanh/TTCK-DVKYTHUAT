using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTCK_DVKYTHUAT.Data;

namespace TTCK_DVKYTHUAT.Components
{
    public class CategoryNewsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryNewsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.CategoryNews.ToListAsync();
            return View(categories);
        }
    }
}
