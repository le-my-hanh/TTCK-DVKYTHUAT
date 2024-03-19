using System.ComponentModel.DataAnnotations;

namespace TTCK_DVKYTHUAT.ModelsView
{
    public class DatLichSuccess
    {
        public int OrderId { get; set; }
     
        public string FullName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ nhận hàng")]
        public DateTime AppServices { get; set; }
        public string Address { get; set; }

    }
}
