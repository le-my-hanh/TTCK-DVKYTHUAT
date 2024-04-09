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
    public class AdminServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminServices
        public  IActionResult Index(int page=1, int CategoryID = 0)
        {
            var pageNumber = page;
            var pageSize = 10;
            List<Service> lsServices = new List<Service>();
            if (CategoryID != 0)
            {
                lsServices = _context.Services
                    .AsNoTracking()
                    .Where(x=>x.CategoryId==CategoryID)
                    .Include(x => x.Category)
                    .OrderByDescending(x => x.CreatedDate).ToList();

            }
            else
            {
                lsServices = _context.Services
                    .AsNoTracking()
                    
                    .Include(x => x.Category)
                    .OrderByDescending(x => x.CreatedDate).ToList();
            }
            PagedList<Service> models = new PagedList<Service>(lsServices.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
            //var applicationDbContext = _context.Services.Include(s => s.Category);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/AdminServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View(service);
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
        // GET: Admin/AdminServices/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Admin/AdminServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,Name,Description,Price,Quantity,Image,CategoryId,CreatedDate")] Service service, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                service.Image = Upload(file);
                service.CreatedDate = DateTime.Now;
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", service.CategoryId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", service.CategoryId);

            return View(service);
        }

        // GET: Admin/AdminServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", service.CategoryId);
            return View(service);
        }

        // POST: Admin/AdminServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ServiceId,Name,Description,Price,Quantity,Image,CategoryId,CreatedDate")] Service service, IFormFile file)
      public async Task<IActionResult> Edit(int id, [FromForm] Service service, IFormFile file)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

           
                try
                {
                    if (file != null)
                    {
                        service.Image = Upload(file);
                    }
                    service.CreatedDate = DateTime.Now;
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", service.CategoryId);
            return View(service);
        }

        // GET: Admin/AdminServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View(service);
        }

        // POST: Admin/AdminServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                
                return Problem("Entity set 'ApplicationDbContext.Services'  is null.");
            }
           
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
               
                _context.Services.Remove(service);
            }
          
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.ServiceId == id)).GetValueOrDefault();
        }
    }
}
