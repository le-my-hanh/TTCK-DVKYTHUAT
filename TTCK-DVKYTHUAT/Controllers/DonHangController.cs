using AspNetCoreHero.ToastNotification.Abstractions;
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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o=>o.TransactStatus)// Nếu có quan hệ với Customer
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound(); // Trả về NotFound nếu không tìm thấy đơn hàng
            }

            var orderDetails = _context.OrderDetails
                .Where(od => od.OrderId == id)
                .Select(od => new
                {
                    OrderDetailId = od.OrderDetailId,
                    ServiceName = od.Service.Name, // Thay "Name" bằng tên thuộc tính chứa tên dịch vụ
                    Quantity = od.Quantity,
                    UnitPrice = od.Service.Price,
                    TotalPrice = od.Quantity * od.Service.Price
                })
                .ToList();

            var response = new
            {
                DonHang = new
                {
                    OrderId = order.OrderId,
                    CustomerName = order.Customer != null ? order.Customer.Name : "Unknown", // Kiểm tra null để tránh lỗi
                    CreatedDate = order.CreatedDate, // Thay bằng tên thuộc tính chứa ngày tạo đơn hàng
                    Status = order.TransactStatus.Status, // Thêm trạng thái đơn hàng
                    OrderDate = order.AppDate // Thêm ngày đặt đơn hàng
                },
                ChiTietDonHang = orderDetails
            };

            return Json(response); // Trả về dữ liệu dưới dạng JSON
        }
        // if (id == null)
        // {
        //     return NotFound();
        // }
        // try
        // {
        //     var taikhoanID = HttpContext.Session.GetString("CustomerId");

        //     if (string.IsNullOrEmpty(taikhoanID)) return RedirectToAction("Login", "Accounts");

        //     var convertedTaikhoanID = Convert.ToInt32(taikhoanID);
        //     var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == convertedTaikhoanID);
        //     var donhang = await _context.Orders
        //         .Include(x => x.TransactStatus)
        //         .FirstOrDefaultAsync(m => m.OrderId == id && convertedTaikhoanID == m.CustomerId);
        //     if (donhang == null)
        //     {
        //         return NotFound();
        //     }

        //     //var chitietdonhang =  _context.OrderDetails
        //     //    .Include(x => x.Service)
        //     //    .AsNoTracking()
        //     //    .Where(x => x.OrderId == id)
        //     //    .OrderBy(x => x.OrderDetailId).ToList();
        //     var chitietdonhang = await _context.OrderDetails
        //.Include(x => x.Service)
        //.AsNoTracking()
        //.Where(x => x.OrderId == id)
        //.OrderBy(x => x.OrderDetailId)
        //.ToListAsync();

        //     XemDonHang donHang = new XemDonHang();
        //     donHang.DonHang = donhang; 
        //     donHang.ChiTietDonHang = chitietdonhang;

        //     // return PartialView("Details",donHang);

        //         return View("Details", donHang); 
        //         //return Json(donHang);


        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine(ex.Message);
        //     return StatusCode(500);
        // }
        // return NotFound();
    }
    
}
