using QuanlyquanNet.Data;

namespace QuanlyquanNet.ViewModel
{
    public class ProfileViewModel
    {
        public string HoTen { get; set; }
        public string TenDangNhap { get; set; }
        public string SoDienThoai { get; set; }
        public decimal SoDu { get; set; }
        public int DiemThuong { get; set; }

        public List<NhiemVuItem> NhiemVuHoanThanh { get; set; } = new();
        public List<PhuThuong> DanhSachPhuThuong { get; set; }
    }

    public class NhiemVuItem
    {
        public string TenNhiemVu { get; set; }
        public int DiemNhan { get; set; }
        public DateTime? NgayHoanThanh { get; set; }
    }
}
