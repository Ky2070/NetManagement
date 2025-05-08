using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class DonHang
{
    public int MaDonHang { get; set; }

    public int MaKhachHang { get; set; }

    public int MaDichVu { get; set; }

    public int? MaKhuyenMai { get; set; }

    public decimal TongTien { get; set; }

    public DateTime? NgayDat { get; set; }

    public string? TrangThai { get; set; }

    public virtual DichVu MaDichVuNavigation { get; set; } = null!;

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
}
