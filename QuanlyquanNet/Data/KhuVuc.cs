using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class KhuVuc
{
    public int MaKhuVuc { get; set; }

    public string TenKhuVuc { get; set; } = null!;

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<MayTinh> MayTinhs { get; set; } = new List<MayTinh>();
}
