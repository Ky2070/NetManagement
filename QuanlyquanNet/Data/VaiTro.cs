using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class VaiTro
{
    public string TenVaiTro { get; set; } = null!;

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
