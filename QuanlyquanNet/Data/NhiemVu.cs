using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class NhiemVu
{
    public int MaNhiemVu { get; set; }

    public string TenNhiemVu { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? MaPhuThuong { get; set; }

    public int? DoKho { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual PhuThuong? MaPhuThuongNavigation { get; set; }
}
