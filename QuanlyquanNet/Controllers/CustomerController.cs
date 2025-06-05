using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using QuanlyquanNet.ViewModel;

namespace QuanlyquanNet.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly QuanLyNetContext _context;
        public CustomerController(QuanLyNetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Profile")]
        public IActionResult Profile()
        {
            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");

            var khach = _context.KhachHangs
                .Include(k => k.KhachHangNhiemVus)
                    .ThenInclude(khv => khv.MaNhiemVuNavigation)
                .FirstOrDefault(k => k.TenDangNhap == tenDangNhap);

            if (khach == null)
            {
                return NotFound();
            }

            // 1. Tính tổng điểm nhiệm vụ đã hoàn thành
            var tongDiem = khach.KhachHangNhiemVus
                .Where(khv => khv.TrangThai == "Hoàn thành")
                .Sum(khv => khv.DiemNhanDuoc ?? 0);

            // 2. Lấy tổng điểm đã dùng để đổi phần thưởng
            var tongDiemDaDoi = _context.PhanThuongDaNhans
                .Where(p => p.MaKhachHang == khach.MaKhachHang)
                .Join(
                    _context.PhuThuongs,
                    p => p.MaPhuThuong,
                    pt => pt.MaPhuThuong,
                    (p, pt) => pt.DiemCanDoi
                ).Sum();

            // 3. Tính điểm còn lại
            var diemThuongConLai = tongDiem - tongDiemDaDoi;

            // 4. Lấy danh sách phần thưởng
            var danhSachPhuThuong = _context.PhuThuongs.ToList();

            // 5. Tạo ViewModel
            var model = new ProfileViewModel
            {
                HoTen = khach.HoTen,
                TenDangNhap = khach.TenDangNhap,
                SoDienThoai = khach.SoDienThoai,
                SoDu = khach.SoDu ?? 0,
                DiemThuong = diemThuongConLai,
                NhiemVuHoanThanh = khach.KhachHangNhiemVus
                    .Where(khv => khv.TrangThai == "Hoàn thành")
                    .Select(khv => new NhiemVuItem
                    {
                        TenNhiemVu = khv.MaNhiemVuNavigation.TenNhiemVu,
                        DiemNhan = khv.DiemNhanDuoc ?? 0,
                        NgayHoanThanh = khv.ThoiGianHoanThanh
                    }).ToList(),
                DanhSachPhuThuong = danhSachPhuThuong
            };

            return View(model);
        }
        [HttpPost("DoiPhanThuong")]
        public IActionResult DoiPhanThuong(string Loai, int Id)
        {
            var khachHang = GetLoggedInKhachHang();
            if (khachHang == null) return RedirectToAction("Login", "Account");

            // Tính tổng điểm thưởng từ nhiệm vụ đã hoàn thành
            var diemThuong = _context.KhachHangNhiemVus
                .Where(khv => khv.MaKhachHang == khachHang.MaKhachHang && khv.TrangThai == "Hoàn thành")
                .Sum(khv => khv.DiemNhanDuoc ?? 0);

            // Lấy tổng điểm đã dùng để đổi quà trước đây
            var tongDiemDaDoi = _context.PhanThuongDaNhans
                .Where(p => p.MaKhachHang == khachHang.MaKhachHang)
                .Join(_context.PhuThuongs, p => p.MaPhuThuong, pt => pt.MaPhuThuong, (p, pt) => pt.DiemCanDoi)
                .Sum();

            var diemConLai = diemThuong - tongDiemDaDoi;

            if (Loai == "PhuThuong")
            {
                var pt = _context.PhuThuongs.Find(Id);
                if (pt != null && diemConLai >= pt.DiemCanDoi)
                {
                    _context.PhanThuongDaNhans.Add(new PhanThuongDaNhan
                    {
                        MaKhachHang = khachHang.MaKhachHang,
                        MaPhuThuong = pt.MaPhuThuong,
                        NgayNhan = DateTime.Now
                    });
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = $"Đã đổi phần thưởng \"{pt.TenPhuThuong}\" thành công! Điểm thưởng còn lại: {diemConLai - pt.DiemCanDoi}";
                }
                else
                {
                    TempData["ErrorMessage"] = $"❌ Không đủ điểm để đổi phần thưởng \"{pt.TenPhuThuong}\". Bạn còn {diemConLai} điểm.";
                }

            }

            return RedirectToAction("Profile");
        }
        private KhachHang? GetLoggedInKhachHang()
        {
            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");
            if (string.IsNullOrEmpty(tenDangNhap))
                return null;

            return _context.KhachHangs.FirstOrDefault(k => k.TenDangNhap == tenDangNhap);
        }
    }
}
