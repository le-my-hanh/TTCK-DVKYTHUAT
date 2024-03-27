using System.ComponentModel.DataAnnotations;

namespace TTCK_DVKYTHUAT.ModelsView
{
    public class DatLichDVVM
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên")]
        public string FullName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ nhận hàng")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày đặt hàng.")]
        [Display(Name = "Ngày Đặt Hàng")]
        public DateTime? AppServices { get; set; }
       
        
       // public int PaymentID { get; set; }
        public string Note { get; set; }
    }
}
