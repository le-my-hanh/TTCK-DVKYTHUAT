using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.Helpers;
using TTCK_DVKYTHUAT.Models;
using TTCK_DVKYTHUAT.ModelsView;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TTCK_DVKYTHUAT.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public INotyfService _notifService { get; }
        public CartController(ApplicationDbContext context, INotyfService notifService)
        {
           
            _context = context;
            _notifService = notifService;
        }
        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
                    if(data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }
        public IActionResult Index()
        {
            
            return View(Carts);
        }
        
    
    //public IActionResult AddtoCart(int id, int? SoLuong)
    //{

    //    try
    //    {
    //        List<CartItem> myCart = Carts;
    //        CartItem item = Carts.SingleOrDefault(p => p.Service.ServiceId == id);

    //        if (item != null)// đã có --> cập nhật số lượng
    //        {
    //            if (SoLuong.HasValue)
    //            {
    //                item.SoLuong = SoLuong.Value;
    //            }
    //            else
    //            {
    //                item.SoLuong++;
    //            }
    //        }
    //        else
    //        {
    //            Service dv = _context.Services.SingleOrDefault(p => p.ServiceId == id);

    //            item = new CartItem
    //            {
    //                SoLuong = SoLuong.HasValue ? SoLuong.Value : 1,
    //                Service = dv,

    //            };
    //            myCart.Add(item);//thêm vào giỏ hàng
    //        }
    //        //lưu lại sesion
    //        HttpContext.Session.Set<List<CartItem>>("GioHang", myCart);
    //        _notifService.Success("Thêm sản phẩm thành công");
    //        //return Json(new { success = true });
    //        return RedirectToAction("Index");
    //    }
    //    catch
    //    {
    //        return Json(new { success = false });
    //    }
    //}

    //[HttpPost]
    //[Route("api/cart/remove")]
    public ActionResult Remove(int serviesID)
       {
            try
            {
                List<CartItem> gioHang = Carts;
                CartItem item = gioHang.SingleOrDefault(p => p.ServiceId == serviesID);
                if (item != null)
                {
                    gioHang.Remove(item);
                }
                //lưu lại session
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);

               // return Json(new { success = true });
                return RedirectToAction("Index");
            }
            catch
            {
                return Json(new { success = false });
            }

      }

        public IActionResult Update(int serviceID, int? SoLuong)
        {
            //lấy giỏ hàng ra để xử lý
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");

            try
            {
                if (cart != null)
                {
                    CartItem item = cart.SingleOrDefault(p => p.ServiceId == serviceID);
                    if (item != null && SoLuong.HasValue)//đã có --cập nhật số lượng
                    {
                        item.SoLuong = SoLuong.Value;
                    }
                    //lưu lại sesion
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                //return Json(new { success = true });
                return RedirectToAction("Index");
            }
            catch
            {
                return Json(new { success = false });

            }
        }

        public IActionResult AddtoCart(int id,int SoLuong, string type="Normal")
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.ServiceId == id);

            if (item == null)
            {
                var sevice = _context.Services.SingleOrDefault(p => p.ServiceId == id);
                item = new CartItem
                {
                    ServiceId = id,
                    TenDV = sevice.Name,
                    DonGia = sevice.Price.Value,
                    SoLuong = 1,
                    Anh = sevice.Image

                };
                myCart.Add(item);
            }
            else
            {
                item.SoLuong+= SoLuong;
            }
            HttpContext.Session.Set("GioHang", myCart);
            if (type == "ajax")
            {
                return Json(new
                {
                    SoLuong = Carts.Sum(P=>P.SoLuong)
                });
            }
            return RedirectToAction("Index");
        }


        //public ActionResult DeleteAll()
        //{

        //    List<CartItem> gioHang = Carts;

        //    if (gioHang != null )
        //    {
        //        gioHang.Clear();
        //        return Json(new { Success = true });
        //    }
        //    return Json(new { Success = false });

        //}

        //asp-action="Increase" asp-controller="Cart" asp-route-id=@item.ServicesId
        public async Task<IActionResult> Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            CartItem cartitem = cart.Where(c => c.ServiceId == id).FirstOrDefault();
            if(cartitem.SoLuong > 1)
            {
                -- cartitem.SoLuong;

            }
            else
            {
                cart.RemoveAll(p => p.ServiceId == id);
            }
            if(cart.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");
            }
            else
            {
                HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(int id)
        {
            List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            CartItem cartitem = cart.Where(c => c.ServiceId == id).FirstOrDefault();
            if (cartitem.SoLuong > 1)
            {
                ++cartitem.SoLuong;

            }
            else
            {
                cart.RemoveAll(p => p.ServiceId == id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");
            }
            else
            {
                HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
            }
            return RedirectToAction("Index");
        }


        //[HttpPost]
        //public async Task<IActionResult> CreateOrder()
        //{
        //    // Lấy giỏ hàng từ session
        //    var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
        //    var customerId = HttpContext.Session.GetString("CustomerId");

        //    if (string.IsNullOrEmpty(customerId))
        //    {
        //        // Người dùng chưa đăng nhập, thực hiện xử lý phù hợp, ví dụ: chuyển hướng đến trang đăng nhập
        //        return RedirectToAction("Login", "Accounts");
        //    }
        //    return View();

        //    //try
        //    //{
        //    //    if (cart != null && cart.Any())
        //    //    {
        //    //        // Kiểm tra xem người dùng đã đăng nhập chưa
        //    //        var taikhoanID = HttpContext.Session.GetString("CustomerId");
        //    //        if (!string.IsNullOrEmpty(taikhoanID))
        //    //        {
        //    //            var customer = await _context.Customers
        //    //                .AsNoTracking()
        //    //                .FirstOrDefaultAsync(c => c.CustomerId == Convert.ToInt32(taikhoanID));

        //    //            if (customer != null)
        //    //            {
        //    //                // Tạo đơn hàng mới
        //    //                var order = new Order
        //    //                {
        //    //                    CustomerId = customer.CustomerId,
        //    //                    CreatedDate = DateTime.Now,
        //    //                    AppDate = DateTime.Now, // Cần cập nhật ngày được chọn từ form đặt hàng
        //    //                    Status = "Chưa xác nhận", // Trạng thái đơn hàng có thể cần điều chỉnh
        //    //                    TotalMoney = cart.Sum(x => x.ThanhTien) // Tính tổng tiền đơn hàng
        //    //                };

        //    //                _context.Orders.Add(order);
        //    //                await _context.SaveChangesAsync();

        //    //                // Thêm chi tiết đơn hàng
        //    //                foreach (var item in cart)
        //    //                {
        //    //                    var orderDetail = new OrderDetail
        //    //                    {
        //    //                        OrderId = order.OrderId,
        //    //                        ServiceId = item.ServiceId,
        //    //                        Quantity = item.SoLuong,
        //    //                        Total = item.ThanhTien
        //    //                    };
        //    //                    _context.OrderDetails.Add(orderDetail);
        //    //                }
        //    //                await _context.SaveChangesAsync();

        //    //                // Xóa giỏ hàng sau khi tạo đơn hàng thành công
        //    //                HttpContext.Session.Remove("GioHang");

        //    //                // Hiển thị thông báo thành công
        //    //                TempData["SuccessMessage"] = "Đơn hàng đã được tạo thành công.";

        //    //                // Chuyển hướng đến trang cần hiển thị sau khi tạo đơn hàng thành công
        //    //                return RedirectToAction("Index", "Home"); // Điều chỉnh đường dẫn và action tùy thuộc vào yêu cầu của ứng dụng của bạn
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            // Nếu người dùng chưa đăng nhập, có thể chuyển hướng đến trang đăng nhập hoặc thực hiện các xử lý khác
        //    //            // Trong trường hợp này, bạn có thể thêm code để xử lý hoặc chuyển hướng đến trang đăng nhập
        //    //            return RedirectToAction("Login", "Accounts"); // Điều chỉnh đường dẫn và action tùy thuộc vào yêu cầu của ứng dụng của bạn
        //    //        }
        //    //    }

        //    //    // Nếu giỏ hàng trống, có thể hiển thị thông báo lỗi hoặc chuyển hướng đến trang cần thiết
        //    //    TempData["ErrorMessage"] = "Giỏ hàng của bạn đang trống.";
        //    //    return RedirectToAction("Index", "Home"); // Điều chỉnh đường dẫn và action tùy thuộc vào yêu cầu của ứng dụng của bạn
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    // Xử lý ngoại lệ nếu có
        //    //    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi tạo đơn hàng.";
        //    //    return RedirectToAction("Index", "Home"); // Điều chỉnh đường dẫn và action tùy thuộc vào yêu cầu của ứng dụng của bạn
        //    //}
        //}
        public IActionResult DeleteAll()
        {
            try
            {
                // Lấy giỏ hàng từ session
                List<CartItem> gioHang = Carts;

                // Xóa tất cả các mục trong giỏ hàng
                gioHang.Clear();

                // Lưu lại giỏ hàng sau khi xóa
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);

                // Chuyển hướng đến trang giỏ hàng sau khi xóa thành công
                return RedirectToAction("Index");
            }
            catch
            {
                // Xử lý nếu có lỗi xảy ra trong quá trình xóa giỏ hàng
                return Json(new { success = false });
            }
        }
    }
}
