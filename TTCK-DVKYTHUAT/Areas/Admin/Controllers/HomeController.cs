using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TTCK_DVKYTHUAT.Data;


namespace TTCK_DVKYTHUAT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public HomeController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }


        public IActionResult Index()
        {

            // Lấy danh sách khách hàng
            var customerCount = _context.Customers.Count();
            // Lấy doanh thu từ hóa đơn
            var totalRevenue = _context.Orders
                .Include(i => i.TransactStatus)
                .Where(i => i.TransactStatus.Status == "Hoàn thành")
                .Sum(i => i.TotalMoney).Value.ToString("#,##0");
            // Lấy số lượng hóa đơn
            var orderCount = _context.Orders.Count();
            // Số lượng hóa đơn hoàn thành
            var ordersuccess = _context.Orders
                .Include(i => i.TransactStatus)
                .Where(i => i.TransactStatus.Status == "Hoàn thành").Count();

            //Số lượng dịch hiện có
            var conmentCount = _context.Conments.Count();

            //số lượng đánh giá tốt 5*
            var rating = _context.Conments
                .Where(i => i.Rating == 5)
                .Count();

            // Gửi dữ liệu đến view thông qua ViewBag
            ViewBag.CustomerCount = customerCount;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.OrderCount = orderCount;
            ViewBag.Ordersuccess = ordersuccess;
            ViewBag.ConmentCount = conmentCount;
            ViewBag.Rating = rating;


            // Lấy dữ liệu hóa đơn đã hoàn thành từ cơ sở dữ liệu
            var completedOrders = _context.Orders
                .Include(o => o.TransactStatus)
                .Where(o => o.TransactStatus.Status == "Hoàn thành" && o.AppDate != null && o.TotalMoney != null)
                .ToList(); // Lấy dữ liệu từ cơ sở dữ liệu và chuyển sang danh sách trên bộ nhớ

            // Nhóm dữ liệu theo tuần sử dụng LINQ to Objects
            var ordersGroupedByWeek = completedOrders
                .GroupBy(o => new { Year = o.AppDate.Value.Year, Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(o.AppDate.Value, CalendarWeekRule.FirstDay, DayOfWeek.Monday) })
                .Select(g => new {
                    Year = g.Key.Year,
                    Week = g.Key.Week,
                    TotalRevenue = g.Sum(o => o.TotalMoney)
                })
                .ToList(); // Chuyển sang danh sách trên bộ nhớ

            // Tạo mảng chứa tuần và doanh thu tương ứng
            var weeks = ordersGroupedByWeek.Select(o => $"Tuần {o.Week} - {o.Year}").ToArray();
            var revenues = ordersGroupedByWeek.Select(o => o.TotalRevenue).ToArray();

            ViewBag.Weeks = weeks;
            ViewBag.Revenues = revenues;


            //// Lấy dữ liệu doanh thu hóa đơn đã ở trạng thái hoàn thành theo ngày
            //var orders = _context.Orders
            //    .Where(o => o.TransactStatus.Status == "Hoàn thành" && o.AppDate != null && o.TotalMoney != null) // Lọc những hóa đơn có ngày hẹn, tổng tiền không null và ở trạng thái hoàn thành
            //    .GroupBy(o => o.AppDate.Value.Date) // Nhóm theo ngày
            //    .Select(g => new {
            //        Date = g.Key,
            //        TotalRevenue = g.Sum(o => o.TotalMoney)
            //    })
            //    .ToList();

            //// Tạo mảng chứa ngày và doanh thu tương ứng
            //var dates = orders.Select(o => o.Date.ToString("yyyy-MM-dd")).ToArray();
            //var revenues = orders.Select(o => o.TotalRevenue).ToArray();

            //// Gửi dữ liệu đến view thông qua ViewBag
            //ViewBag.Dates = dates;
            //ViewBag.Revenues = revenues;


            return View();

        }
       





    }
    
}
