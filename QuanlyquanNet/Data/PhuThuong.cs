using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class PhuThuong
{
    public int MaPhuThuong { get; set; }

    public string TenPhuThuong { get; set; } = null!;

    public int DiemCanDoi { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<NhiemVu> NhiemVus { get; set; } = new List<NhiemVu>();

    public virtual ICollection<PhanThuongDaNhan> PhanThuongDaNhans { get; set; } = new List<PhanThuongDaNhan>();
}
