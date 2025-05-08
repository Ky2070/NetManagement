using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class LichSuDangNhap
{
    public int MaDangNhap { get; set; }

    public int MaNguoiDung { get; set; }

    public DateTime? ThoiGianDangNhap { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
}
