using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Collections.Generic;
using TTCK_DVKYTHUAT.Data;
using TTCK_DVKYTHUAT.Helpers;
using TTCK_DVKYTHUAT.Models;
using TTCK_DVKYTHUAT.ModelsView;

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


    }
}
