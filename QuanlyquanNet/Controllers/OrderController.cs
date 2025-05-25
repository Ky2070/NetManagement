using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using QuanlyquanNet.ViewModel;

namespace QuanlyquanNet.Controllers
{
    [Authorize(Roles = "Employee")]
    public class OrderController : Controller
    {
        private readonly QuanLyNetContext _context;

        public OrderController(QuanLyNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult NhapDonHang()
        {
            var model = new TaoDonHangViewModel
            {
                DanhSachKhachHang = _context.KhachHangs.Select(k => new SelectListItem
                {
                    Value = k.MaKhachHang.ToString(),
                    Text = $"{k.HoTen} - {k.TenDangNhap}"
                }).ToList(),
                DanhSachDichVu = _context.DichVus.Select(d => new DichVuItemViewModel
                {
                    MaDichVu = d.MaDichVu,
                    TenDichVu = d.TenDichVu,
                    Gia = d.Gia,
                    HinhAnh = d.HinhAnh
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult NhapDonHang(TaoDonHangViewModel model)
        {
            var khach = _context.KhachHangs.Include(k => k.PhienChois)
                .FirstOrDefault(k => k.MaKhachHang == model.MaKhachHang);
            var dichVu = _context.DichVus.Find(model.MaDichVu);

            if (khach == null || dichVu == null)
            {
                TempData["error"] = "Không tìm thấy thông tin khách hoặc dịch vụ.";
                return RedirectToAction("NhapDonHang");
            }

            var mayTinhId = khach.PhienChois
                            .OrderByDescending(p => p.ThoiGianBatDau)
                            .FirstOrDefault()?.MaMayTinh;

            if (mayTinhId == null || !_context.MayTinhs.Any(m => m.MaMayTinh == mayTinhId))
            {
                TempData["error"] = "Khách hàng chưa có phiên chơi hoặc không rõ vị trí máy tính.";
                return RedirectToAction("NhapDonHang");
            }

            var donHang = new DonHang
            {
                MaKhachHang = khach.MaKhachHang,
                MaDichVu = dichVu.MaDichVu,
                SoLuong = model.SoLuong,
                MaMayTinh = (int)mayTinhId,
                ThongTinViTri = $"Máy số {mayTinhId}" + (string.IsNullOrWhiteSpace(model.GhiChu) ? "" : $" - {model.GhiChu}"),
                TongTien = dichVu.Gia * model.SoLuong,
                NgayDat = DateTime.Now,
                TrangThai = "Chờ giao"
            };

            // Nếu cấu hình trừ tiền tự động:
            if (khach.SoDu >= donHang.TongTien)
            {
                khach.SoDu -= donHang.TongTien;
                donHang.TrangThai = "Đã giao"; // Nếu muốn tự động giao luôn
            }

            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            TempData["success"] = "✅ Đơn hàng đã được tạo thành công.";
            return RedirectToAction("NhapDonHang");
        }
    }
}
