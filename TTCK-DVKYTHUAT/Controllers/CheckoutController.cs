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
        public IActionResult Index(string returnUrl = null)
        {
            //lấy giỏ hàng ra xử lý
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            DatLichDVVM model = new DatLichDVVM();
            if (taikhoanID != null)
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
        public async Task<IActionResult> Index(DatLichDVVM model)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taikhoanID = HttpContext.Session.GetString("CustomerId");
            //DatLichDVVM model = new DatLichDVVM();
            if (taikhoanID != null)
            {
                var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
                model.CustomerId = khachhang.CustomerId;
                model.FullName = khachhang.Name;
                model.Email = khachhang.Email;
                model.Phone = khachhang.Phone;
                model.Address = khachhang.Address;
                //ViewBag.GioHang = cart;

                _context.Update(khachhang);
                _context.SaveChanges();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    //khởi tạo đơn hàng
                    Order donhang = new Order();
                    donhang.CustomerId = model.CustomerId;
                    donhang.Customer.Address = model.Address;
                    donhang.Customer.Phone = model.Phone;
                    donhang.CreatedDate = DateTime.Now;
                    donhang.AppDate = model.AppServices;
                    donhang.Payment.PaymentId = model.PaymentID;
                    donhang.Status = model.Note;
                    donhang.TotalMoney = Convert.ToInt32(cart.Sum(x => x.ThanhTien));

                    _context.Add(donhang);
                    _context.SaveChanges();

                    //Tạo danh sách đơn hàng

                    foreach (var item in cart)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = donhang.OrderId;
                        orderDetail.ServiceId = item.ServiceId;
                        orderDetail.Quantity = item.SoLuong;
                        orderDetail.Total = donhang.TotalMoney;
                        orderDetail.Service.Price = (int?)item.DonGia;
                        orderDetail.CreatDate = DateTime.Now;
                        _context.Add(orderDetail);
                    }
                    _context.SaveChanges();
                    //Clear giỏ hàng
                    HttpContext.Session.Remove("GioHang");
                    //xuất thông báo
                    _notifService.Success("Đơn hàng đặt thành công");
                    //cập nhập thông báo khách hàng
                    return RedirectToAction("Index", "Cart");
                }

            }
            catch (Exception ex)
            {
               

            }
            return View(model);
        }
        //[Route("dat-hang-thanh-cong.html", Name = "Success")]
        //public IActionResult Success()
        //{
        //    try
        //    {
        //        var taikhoanID = HttpContext.Session.GetString("CustomerId");
        //        if (string.IsNullOrEmpty(taikhoanID))
        //        {
        //            return RedirectToAction("Login", "Accounts", new { returnUrl = "/dat-hang-thanh-cong.html" });
        //        }
        //        var khachhang = _context.Customers.AsNoTracking().SingleOrDefault(x => x.CustomerId == Convert.ToInt32(taikhoanID));
        //        var donhang = _context.Orders.Where(x => x.CustomerId == Convert.ToInt32(taikhoanID)).OrderByDescending(x => x.CreatedDate);
        //        DatLichSuccess successVM = new DatLichSuccess();
        //        successVM.FullName = khachhang.Name;
        //        //successVM.OrderId = donhang.OrderId;
        //        successVM.Phone = khachhang.Phone;
        //        successVM.Address = khachhang.Address;
        //        //successVM.AppServices = donhang.;
        //        return View(successVM);
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


    }
}
