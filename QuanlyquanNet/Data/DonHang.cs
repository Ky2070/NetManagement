using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class DonHang
{
    public int MaDonHang { get; set; }

    public int MaKhachHang { get; set; }

    public int MaDichVu { get; set; }

    public int SoLuong { get; set; }

    public int MaMayTinh { get; set; }

    public string ThongTinViTri { get; set; } = null!;

    public decimal TongTien { get; set; }

    public DateTime? NgayDat { get; set; }

    public string? TrangThai { get; set; }

    public virtual DichVu MaDichVuNavigation { get; set; } = null!;

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual MayTinh MaMayTinhNavigation { get; set; } = null!;
}
