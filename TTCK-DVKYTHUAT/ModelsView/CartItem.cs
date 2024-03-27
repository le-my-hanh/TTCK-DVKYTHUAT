using TTCK_DVKYTHUAT.Models;

namespace TTCK_DVKYTHUAT.ModelsView
{
    public class CartItem
    {
        //{
        //    public Service Service { get; set; }

        //    public int SoLuong { get; set; }
        //    public double ThanhTien => SoLuong * Service.Price.Value;
        public int ServiceId { get; set; }
        public string TenDV { get; set; }
        public string Anh { get; set; }
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien => SoLuong * DonGia;

    }
}

