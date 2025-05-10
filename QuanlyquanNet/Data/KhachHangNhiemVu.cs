using System;
using System.Collections.Generic;

namespace QuanlyquanNet.Data;

public partial class KhachHangNhiemVu
{
    public int MaKhachHang { get; set; }

    public int MaNhiemVu { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? ThoiGianHoanThanh { get; set; }

    public int? DiemNhanDuoc { get; set; }

    public DateTime? NgayThamGia { get; set; }

    public virtual KhachHang MaKhachHangNavigation { get; set; } = null!;

    public virtual NhiemVu MaNhiemVuNavigation { get; set; } = null!;
}
