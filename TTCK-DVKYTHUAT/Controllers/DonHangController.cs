﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.ModelsView;

namespace TTCK_DVKYTHUAT.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public INotyfService _notifService { get; }
        public DonHangController(ApplicationDbContext context, INotyfService notifService)
        {

            _context = context;
            _notifService = notifService;
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanID)) return RedirectToAction("Login", "Accounts");
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                var donhang = await _context.Orders
                    .Include(x => x.TransactStatus)
                    .FirstOrDefaultAsync(m => m.OrderId == id && Convert.ToInt32(taikhoanID) == m.CustomerId);
                if (donhang == null)
                {
                    return NotFound();
                }

                var chitietdonhang = _context.OrderDetails
                    .Include(x => x.Service)
                    .AsNoTracking()
                    .Where(x => x.OrderId == id)
                    .OrderBy(x => x.OrderDetailId).ToList();

                XemDonHang donHang = new XemDonHang();
                donHang.DonHang = donhang; 
                donHang.ChiTietDonHang = chitietdonhang; 
                return PartialView("Details", donHang);
                    
            }
            catch 
            {

            }
            return NotFound();
        }
    }
}
