using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class VatPham
{
    public int MaVatPham { get; set; }

    public string TenVatPham { get; set; } = null!;

    public string? MoTa { get; set; }

    public int DiemCanDoi { get; set; }

    public decimal? GiaMua { get; set; }

    public int SoLuongTon { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<PhuThuong> PhuThuongs { get; set; } = new List<PhuThuong>();
}
