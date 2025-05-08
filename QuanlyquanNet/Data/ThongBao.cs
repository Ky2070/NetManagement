using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class ThongBao
{
    public int MaThongBao { get; set; }

    public int? MaKhachHang { get; set; }

    public int? MaNhanVien { get; set; }

    public string NoiDung { get; set; } = null!;

    public string? TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual NhanVien? MaNhanVienNavigation { get; set; }
}
