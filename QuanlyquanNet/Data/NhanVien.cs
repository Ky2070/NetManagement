using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class NhanVien
{
    public int MaNhanVien { get; set; }

    public int MaNguoiDung { get; set; }

    public string HoTen { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
