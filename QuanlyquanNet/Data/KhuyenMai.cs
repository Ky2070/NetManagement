using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class KhuyenMai
{
    public int MaKhuyenMai { get; set; }

    public string TenKhuyenMai { get; set; } = null!;

    public decimal? PhanTramGiamGia { get; set; }

    public DateTime? NgayBatDau { get; set; }

    public DateTime? NgayKetThuc { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
