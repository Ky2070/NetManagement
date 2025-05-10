using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string TenVaiTro { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public virtual KhachHang? KhachHang { get; set; }

    public virtual ICollection<LichSuDangNhap> LichSuDangNhaps { get; set; } = new List<LichSuDangNhap>();

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();

    public virtual VaiTro TenVaiTroNavigation { get; set; } = null!;
}
