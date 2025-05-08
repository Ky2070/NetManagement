using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class PhanThuongDaNhan
{
    public int MaPhanThuong { get; set; }

    public int MaKhachHang { get; set; }

    public int MaPhuThuong { get; set; }

    public DateTime? NgayNhan { get; set; }

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual PhuThuong MaPhuThuongNavigation { get; set; } = null!;
}
