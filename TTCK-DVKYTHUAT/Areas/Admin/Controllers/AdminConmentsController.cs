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
    public class AdminConmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminConmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminConments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Conments.Include(c => c.Customer).Include(c => c.Service);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/AdminConments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Conments == null)
            {
                return NotFound();
            }

            var conment = await _context.Conments
                .Include(c => c.Customer)
                .Include(c => c.Service)
                .FirstOrDefaultAsync(m => m.ConmentId == id);
            if (conment == null)
            {
                return NotFound();
            }

            return View(conment);
        }

        // GET: Admin/AdminConments/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId");
            return View();
        }

        // POST: Admin/AdminConments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConmentId,ServiceId,CustomerId,Message,Rating")] Conment conment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", conment.CustomerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", conment.ServiceId);
            return View(conment);
        }

        // GET: Admin/AdminConments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Conments == null)
            {
                return NotFound();
            }

            var conment = await _context.Conments.FindAsync(id);
            if (conment == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", conment.CustomerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", conment.ServiceId);
            return View(conment);
        }

        // POST: Admin/AdminConments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConmentId,ServiceId,CustomerId,Message,Rating")] Conment conment)
        {
            if (id != conment.ConmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConmentExists(conment.ConmentId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", conment.CustomerId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", conment.ServiceId);
            return View(conment);
        }

        // GET: Admin/AdminConments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Conments == null)
            {
                return NotFound();
            }

            var conment = await _context.Conments
                .Include(c => c.Customer)
                .Include(c => c.Service)
                .FirstOrDefaultAsync(m => m.ConmentId == id);
            if (conment == null)
            {
                return NotFound();
            }

            return View(conment);
        }

        // POST: Admin/AdminConments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Conments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Conments'  is null.");
            }
            var conment = await _context.Conments.FindAsync(id);
            if (conment != null)
            {
                _context.Conments.Remove(conment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConmentExists(int id)
        {
          return (_context.Conments?.Any(e => e.ConmentId == id)).GetValueOrDefault();
        }
    }
}
