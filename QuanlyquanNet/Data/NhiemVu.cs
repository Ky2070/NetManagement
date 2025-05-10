using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class NhiemVu
{
    public int MaNhiemVu { get; set; }

    public string TenNhiemVu { get; set; } = null!;

    public string? MoTa { get; set; }

    public string? DieuKien { get; set; }

    public int DiemThuong { get; set; }

    public int? DoKho { get; set; }

    public string? HinhAnh { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<KhachHangNhiemVu> KhachHangNhiemVus { get; set; } = new List<KhachHangNhiemVu>();
}
