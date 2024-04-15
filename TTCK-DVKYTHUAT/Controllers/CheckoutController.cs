using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.Helpers;
using TTCK_DVKYTHUAT.Models;
using TTCK_DVKYTHUAT.ModelsView;

namespace TTCK_DVKYTHUAT.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public INotyfService _notifService { get; }
        public CheckoutController(ApplicationDbContext context, INotyfService notifService)
        {

            _context = context;
            _notifService = notifService;
        }
        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }
      
        [Route("checkout.html", Name = "Checkout")]
        public IActionResult Index()
        {

            //lấy giỏ hàng ra xử lý
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            DatLichDVVM model = new DatLichDVVM();
            if (string.IsNullOrEmpty(taikhoanID))
            {
                // Người dùng chưa đăng nhập, thực hiện xử lý phù hợp, ví dụ: chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.Name;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;
            }
           
            ViewBag.GioHang = cart;
            return View(model);
        }
      
        [HttpPost]
        [Route("checkout.html", Name = "Checkout")]
        public async Task<IActionResult> Index(DatLichDVVM datLich)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(datLich.Address) || !datLich.AppServices.HasValue)
                {
                    ModelState.AddModelError("", "Vui lòng nhập đầy đủ địa chỉ và thời gian đặt.");
                    return View(datLich);
                }
                //lấy giỏ hàng ra xử lý
                var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
                var taikhoanID = HttpContext.Session.GetString("CustomerId");
                if (string.IsNullOrEmpty(taikhoanID))
                {
                    // Người dùng chưa đăng nhập, thực hiện xử lý phù hợp, ví dụ: chuyển hướng đến trang đăng nhập
                    return RedirectToAction("Login", "Accounts");
                }
                else
                {
                    var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                    if(khachhang != null)
                    {
                        khachhang.Address = datLich.Address;
                        _context.Update(khachhang);
                        _context.SaveChanges();
                    }
                    
                }
                var donhang = new Order
                {
                    CustomerId = Convert.ToInt32(taikhoanID),
                    AppDate = datLich.AppServices.Value,
                    CreatedDate = DateTime.Now,
                    Note = datLich.Note,
                    TotalMoney = Convert.ToInt32(cart.Sum(x => x.ThanhTien)),
                    PaymentId = null,
                    TransactStatusId = 1,

                };
                _context.Add(donhang);
                _context.SaveChanges();

                foreach (var item in cart)
                {
                    var ct = new OrderDetail()
                    {
                        CreatDate = DateTime.Now,
                    OrderId = donhang.OrderId,
                    ServiceId = item.ServiceId,
                    Quantity = item.SoLuong,
                    Total = donhang.TotalMoney,
                    };
                   
                   
                    _context.Add(ct);
                    _context.SaveChanges();
                }
                //Clear giỏ hàng
                HttpContext.Session.Remove("GioHang");
                //xuất thông báo
                 _notifService.Success("Đơn hàng đặt thành công");
               
                //cập nhập thông báo khách hàng
                return RedirectToAction("Index", "Service");

            }
            return View(datLich);

        }

        //[HttpPost]
        //public async Task<IActionResult> CancelOrder(int orderId)
        //{
        //    var order = await _context.Orders.FindAsync(orderId);
        //    if (order == null)
        //    {
        //        return NotFound(); // Order not found
        //    }

        //    // Remove order details associated with the order
        //    var orderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId);
        //    _context.OrderDetails.RemoveRange(orderDetails);

        //    // Remove the order itself
        //    _context.Orders.Remove(order);

        //    await _context.SaveChangesAsync();

        //    // Return a JSON response indicating success
        //    return Json(new { success = true, message = "Đơn hàng đã được hủy thành công." });
        //}
        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound(); // Order not found
            }

            if (order.TransactStatusId != 1) // Kiểm tra nếu đơn hàng không ở trạng thái "chờ xác nhận"
            {
                return Json(new { success = false, message = "Bạn không có quyền hủy đơn hàng này." });
            }

            // Xóa chi tiết đơn hàng liên quan đến đơn hàng
            var orderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId);
            _context.OrderDetails.RemoveRange(orderDetails);

            // Remove the order itself
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            // Return a JSON response indicating success
            return Json(new { success = true, message = "Đơn hàng đã được hủy thành công." });
        }
    }
}
