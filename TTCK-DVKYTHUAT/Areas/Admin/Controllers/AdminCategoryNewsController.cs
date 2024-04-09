using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.Models;

namespace TTCK_DVKYTHUAT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCategoryNewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminCategoryNewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminCategoryNews
        public async Task<IActionResult> Index()
        {
              return _context.CategoryNews != null ? 
                          View(await _context.CategoryNews.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.CategoryNews'  is null.");
        }

        public string Upload(IFormFile file)
        {
            string uploadFileName = null;
            if (file != null)
            {

                uploadFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var path = $"wwwroot\\images\\{uploadFileName}";
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }
            return uploadFileName;
        }

        // GET: Admin/AdminCategoryNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryNews == null)
            {
                return NotFound();
            }

            var categoryNews = await _context.CategoryNews
                .FirstOrDefaultAsync(m => m.CategorynewId == id);
            if (categoryNews == null)
            {
                return NotFound();
            }

            return View(categoryNews);
        }

        // GET: Admin/AdminCategoryNews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCategoryNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategorynewId,Name,Image,CreatedDate,ShortDesc")] CategoryNews categoryNews, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                categoryNews.Image = Upload(file);
                categoryNews.CreatedDate = DateTime.Now;
                _context.Add(categoryNews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryNews);
        }

        // GET: Admin/AdminCategoryNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryNews == null)
            {
                return NotFound();
            }

            var categoryNews = await _context.CategoryNews.FindAsync(id);
            if (categoryNews == null)
            {
                return NotFound();
            }
            return View(categoryNews);
        }

        // POST: Admin/AdminCategoryNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategorynewId,Name,Image,CreatedDate,ShortDesc")] CategoryNews categoryNews, IFormFile file)
        {
            if (id != categoryNews.CategorynewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        categoryNews.Image = Upload(file);
                    }
                    _context.Update(categoryNews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryNewsExists(categoryNews.CategorynewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoryNews);
        }

        // GET: Admin/AdminCategoryNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryNews == null)
            {
                return NotFound();
            }

            var categoryNews = await _context.CategoryNews
                .FirstOrDefaultAsync(m => m.CategorynewId == id);
            if (categoryNews == null)
            {
                return NotFound();
            }

            return View(categoryNews);
        }

        // POST: Admin/AdminCategoryNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryNews == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CategoryNews'  is null.");
            }
            var categoryNews = await _context.CategoryNews.FindAsync(id);
            if (categoryNews != null)
            {
                _context.CategoryNews.Remove(categoryNews);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryNewsExists(int id)
        {
          return (_context.CategoryNews?.Any(e => e.CategorynewId == id)).GetValueOrDefault();
        }
    }
}
