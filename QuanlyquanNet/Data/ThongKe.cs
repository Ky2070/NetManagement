using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class ThongKe
{
    public int MaThongKe { get; set; }

    public DateOnly? NgayThongKe { get; set; }

    public decimal? TongDoanhThu { get; set; }

    public int? SoMayHoatDong { get; set; }

    public int? TongSoPhien { get; set; }

    public DateTime? NgayTao { get; set; }
}
