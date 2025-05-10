using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class PhienChoi
{
    public int MaPhienChoi { get; set; }

    public int MaKhachHang { get; set; }

    public int MaMayTinh { get; set; }

    public string ThongTinViTri { get; set; } = null!;

    public DateTime? ThoiGianBatDau { get; set; }

    public DateTime? ThoiGianKetThuc { get; set; }

    public decimal? TongChiPhi { get; set; }

    public string? TrangThai { get; set; }

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual MayTinh MaMayTinhNavigation { get; set; } = null!;
}
