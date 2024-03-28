using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.Models;

namespace TTCK_DVKYTHUAT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminNewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminNewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminNews
        public async Task<IActionResult> Index(int page = 1, int CategoryNewsID = 0)
        {
            var pageNumber = page;
            var pageSize = 5;
            List<News> lsNews = new List<News>();
            if (CategoryNewsID != 0)
            {
                lsNews = _context.News
                    .AsNoTracking()
                    .Where(x => x.CategorynewId == CategoryNewsID)
                    .Include(x => x.Categorynew)
                    .OrderByDescending(x => x.CreatedDate).ToList();

            }
            else
            {
                lsNews = _context.News
                    .AsNoTracking()

                    .Include(x => x.Categorynew)
                    .OrderByDescending(x => x.CreatedDate).ToList();
            }
            PagedList<News> models = new PagedList<News>(lsNews.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
            //var applicationDbContext = _context.News.Include(n => n.Categorynew);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/AdminNews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Categorynew)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
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
        // GET: Admin/AdminNews/Create
        public IActionResult Create()
        {
            ViewData["CategorynewId"] = new SelectList(_context.CategoryNews, "CategorynewId", "Name");
            return View();
        }

        // POST: Admin/AdminNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsId,Title,Image,Desciption,CreatedDate,CategorynewId")] News news, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                news.Image = Upload(file);
                _context.Add(news);
                news.CreatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorynewId"] = new SelectList(_context.CategoryNews, "CategorynewId", "Name", news.CategorynewId);
            return View(news);
        }

        // GET: Admin/AdminNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["CategorynewId"] = new SelectList(_context.CategoryNews, "CategorynewId", "Name", news.CategorynewId);
            return View(news);
        }

        // POST: Admin/AdminNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,Title,Image,Desciption,CreatedDate,CategorynewId")] News news, IFormFile file)
        {
            if (id != news.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        news.Image = Upload(file);
                    }
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.NewsId))
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
            ViewData["CategorynewId"] = new SelectList(_context.CategoryNews, "CategorynewId", "Name", news.CategorynewId);
            return View(news);
        }

        // GET: Admin/AdminNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Categorynew)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: Admin/AdminNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'ApplicationDbContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
          return (_context.News?.Any(e => e.NewsId == id)).GetValueOrDefault();
        }
    }
}
