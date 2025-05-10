using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class GoiNap
{
    public int MaGoiNap { get; set; }

    public string TenGoi { get; set; } = null!;

    public decimal SoTien { get; set; }

    public int? MaKhuyenMai { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<LichSuNapTien> LichSuNapTiens { get; set; } = new List<LichSuNapTien>();

    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
}
