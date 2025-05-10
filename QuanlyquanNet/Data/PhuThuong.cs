using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class PhuThuong
{
    public int MaPhuThuong { get; set; }

    public string TenPhuThuong { get; set; } = null!;

    public string? LoaiPhuThuong { get; set; }

    public decimal? GiaTri { get; set; }

    public int DiemCanDoi { get; set; }

    public int? MaVatPham { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual VatPham? MaVatPhamNavigation { get; set; }

    public virtual ICollection<PhanThuongDaNhan> PhanThuongDaNhans { get; set; } = new List<PhanThuongDaNhan>();
}
