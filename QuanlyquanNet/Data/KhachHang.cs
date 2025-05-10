using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string SoDienThoai { get; set; } = null!;

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string? LoaiTaiKhoan { get; set; }

    public decimal? SoDu { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<KhachHangNhiemVu> KhachHangNhiemVus { get; set; } = new List<KhachHangNhiemVu>();

    public virtual ICollection<LichSuNapTien> LichSuNapTienMaKhachHangNavigations { get; set; } = new List<LichSuNapTien>();

    public virtual ICollection<LichSuNapTien> LichSuNapTienTenDangNhapNavigations { get; set; } = new List<LichSuNapTien>();

    public virtual ICollection<PhanThuongDaNhan> PhanThuongDaNhans { get; set; } = new List<PhanThuongDaNhan>();

    public virtual ICollection<PhienChoi> PhienChois { get; set; } = new List<PhienChoi>();

    public virtual NguoiDung TenDangNhapNavigation { get; set; } = null!;

    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
