


using TTCK_DVKYTHUAT.Models;

namespace TTCK_DVKYTHUAT.ModelsView
{
    public class XemDonHang
    {
        public Order DonHang { get; set; }
        public List<OrderDetail> ChiTietDonHang { get; set; }
    }
}
