using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class MayTinh
{
    public int MaMayTinh { get; set; }
    public int MaKhuVuc { get; set; }
    public string Stt { get; set; } = null!;
    public string TenMayTinh { get; set; } = null!;
    public string? TrangThai { get; set; }
    public DateTime? NgayTao { get; set; }
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
    [BindNever]
    public virtual KhuVuc MaKhuVucNavigation { get; set; }
    public virtual ICollection<PhienChoi> PhienChois { get; set; } = new List<PhienChoi>();
    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}