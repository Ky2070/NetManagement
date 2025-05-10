using System.ComponentModel.DataAnnotations;

public class EditNhanVienViewModel
{
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string HoTen { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Display(Name = "Số điện thoại")]
    public string SoDienThoai { get; set; }
}
