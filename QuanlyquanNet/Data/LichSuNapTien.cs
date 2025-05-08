using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class LichSuNapTien
{
    public int MaNapTien { get; set; }

    public int MaKhachHang { get; set; }

    public decimal SoTien { get; set; }

    public DateTime? NgayNap { get; set; }

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;
}
