using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class DichVu
{
    public int MaDichVu { get; set; }

    public string TenDichVu { get; set; } = null!;

    public decimal Gia { get; set; }

    public string? LoaiDichVu { get; set; }

    public string? HinhAnh { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
