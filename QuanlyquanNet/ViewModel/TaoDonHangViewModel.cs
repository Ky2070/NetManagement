using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanlyquanNet.ViewModel
{
    public class TaoDonHangViewModel
    {
        public int MaKhachHang { get; set; }
        public int MaDichVu { get; set; }
        public int SoLuong { get; set; }
        public string? GhiChu { get; set; }
        public List<SelectListItem> DanhSachKhachHang { get; set; } = new();
        public List<DichVuItemViewModel> DanhSachDichVu { get; set; } = new();
    }
}
