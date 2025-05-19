namespace QuanlyquanNet.ViewModel
{
    public class AssignedTaskViewModel
    {
        public int Id { get; set; } // => KhachHangNhiemVu Id
        public string KhachHang { get; set; }
        public string Game { get; set; }
        public string TenNhiemVu { get; set; }
        public string MoTa { get; set; }
        public int DiemThuong { get; set; }
        public DateTime? NgayThamGia { get; set; }
        public string TrangThai { get; set; }
    }

}
