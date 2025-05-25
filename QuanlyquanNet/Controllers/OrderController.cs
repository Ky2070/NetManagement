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

            if (khach == null)
            {
                TempData["error"] = "Không tìm thấy khách hàng.";
                return RedirectToAction("NhapDonHang");
            }

            var mayTinhId = khach.PhienChois.OrderByDescending(p => p.ThoiGianBatDau)
                                             .FirstOrDefault()?.MaMayTinh;

            if (mayTinhId == null)
            {
                TempData["error"] = "Không xác định được vị trí máy tính.";
                return RedirectToAction("NhapDonHang");
            }

            foreach (var item in model.DonHangChiTiets)
            {
                if (item.SoLuong <= 0) continue;

                var dichVu = _context.DichVus.Find(item.MaDichVu);
                if (dichVu == null) continue;

                var tongTien = item.SoLuong * dichVu.Gia;

                var donHang = new DonHang
                {
                    MaKhachHang = khach.MaKhachHang,
                    MaDichVu = dichVu.MaDichVu,
                    SoLuong = item.SoLuong,
                    MaMayTinh = (int)mayTinhId,
                    ThongTinViTri = $"Máy số {mayTinhId}" + (string.IsNullOrWhiteSpace(item.GhiChu) ? "" : $" - {item.GhiChu}"),
                    TongTien = tongTien,
                    NgayDat = DateTime.Now,
                };

                if (khach.SoDu >= tongTien)
                {
                    khach.SoDu -= tongTien;
                    donHang.TrangThai = "Đã giao";
                }
                else
                {
                    donHang.TrangThai = "Trả tiền mặt";
                }

                _context.DonHangs.Add(donHang);
            }

            _context.SaveChanges();
            TempData["success"] = "✅ Đã tạo đơn hàng thành công.";
            return RedirectToAction("NhapDonHang");
        }

    }
}
