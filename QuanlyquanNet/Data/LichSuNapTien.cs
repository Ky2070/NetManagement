using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class LichSuNapTien
{
    public int MaNapTien { get; set; }

    public int MaKhachHang { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public int MaGoiNap { get; set; }

    public decimal SoTien { get; set; }

    public decimal SoTienThucNhan { get; set; }

    public DateTime? NgayNap { get; set; }

    public string? TrangThai { get; set; }

    public virtual GoiNap MaGoiNapNavigation { get; set; } = null!;

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual KhachHang TenDangNhapNavigation { get; set; } = null!;
}
