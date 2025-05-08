using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string HoTen { get; set; } = null!;

    public decimal? SoDu { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<LichSuNapTien> LichSuNapTiens { get; set; } = new List<LichSuNapTien>();

    public virtual ICollection<PhanThuongDaNhan> PhanThuongDaNhans { get; set; } = new List<PhanThuongDaNhan>();

    public virtual ICollection<PhienChoi> PhienChois { get; set; } = new List<PhienChoi>();

    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
