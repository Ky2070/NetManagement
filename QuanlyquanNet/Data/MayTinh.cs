using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class MayTinh
{
    public int MaMayTinh { get; set; }

    public int MaKhuVuc { get; set; }

    public string TenMayTinh { get; set; } = null!;

    public bool? TrangThai { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual KhuVuc MaKhuVucNavigation { get; set; } = null!;

    public virtual ICollection<PhienChoi> PhienChois { get; set; } = new List<PhienChoi>();
}
