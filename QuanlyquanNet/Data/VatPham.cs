using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class VatPham
{
    public int MaVatPham { get; set; }

    public string TenVatPham { get; set; } = null!;

    public int DiemCanDoi { get; set; }

    public DateTime? NgayTao { get; set; }
}
