using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class LichSuDangNhap
{
    public int MaDangNhap { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string VaiTro { get; set; } = null!;

    public DateTime? ThoiGianDangNhap { get; set; }

    public virtual NguoiDung TenDangNhapNavigation { get; set; } = null!;
}
