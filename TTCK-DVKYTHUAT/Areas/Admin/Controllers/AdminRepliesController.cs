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
    public class AdminRepliesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminRepliesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminReplies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Replys.Include(r => r.Conment).ThenInclude(c => c.Customer);
                                    
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/AdminReplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Replys == null)
            {
                return NotFound();
            }

            var reply = await _context.Replys
                .Include(r => r.Conment)
                .FirstOrDefaultAsync(m => m.ReplyId == id);
            if (reply == null)
            {
                return NotFound();
            }

            return View(reply);
        }

        // GET: Admin/AdminReplies/Create
        public IActionResult Create()
        {
            ViewData["ConmentId"] = new SelectList(_context.Conments, "ConmentId", "ConmentId");
            return View();
        }

        // POST: Admin/AdminReplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReplyId,Reply1,RepDate,ConmentId")] Reply reply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConmentId"] = new SelectList(_context.Conments, "ConmentId", "ConmentId", reply.ConmentId);
            return View(reply);
        }

        // GET: Admin/AdminReplies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Replys == null)
            {
                return NotFound();
            }

            var reply = await _context.Replys.FindAsync(id);
            if (reply == null)
            {
                return NotFound();
            }
            ViewData["ConmentId"] = new SelectList(_context.Conments, "ConmentId", "ConmentId", reply.ConmentId);
            return View(reply);
        }

        // POST: Admin/AdminReplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReplyId,Reply1,RepDate,ConmentId")] Reply reply)
        {
            if (id != reply.ReplyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReplyExists(reply.ReplyId))
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
            ViewData["ConmentId"] = new SelectList(_context.Conments, "ConmentId", "ConmentId", reply.ConmentId);
            return View(reply);
        }

        // GET: Admin/AdminReplies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Replys == null)
            {
                return NotFound();
            }

            var reply = await _context.Replys
                .Include(r => r.Conment)
                .FirstOrDefaultAsync(m => m.ReplyId == id);
            if (reply == null)
            {
                return NotFound();
            }

            return View(reply);
        }

        // POST: Admin/AdminReplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Replys == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Replys'  is null.");
            }
            var reply = await _context.Replys.FindAsync(id);
            if (reply != null)
            {
                _context.Replys.Remove(reply);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReplyExists(int id)
        {
          return (_context.Replys?.Any(e => e.ReplyId == id)).GetValueOrDefault();
        }
    }
}
