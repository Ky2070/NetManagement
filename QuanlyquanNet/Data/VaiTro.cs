﻿using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class VaiTro
{
    public int MaVaiTro { get; set; }

    public string? TenVaiTro { get; set; }

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
