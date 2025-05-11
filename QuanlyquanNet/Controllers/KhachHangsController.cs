using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    [Route("[controller]")]
    public class KhachHangsController : Controller
    {
        private readonly QuanLyNetContext _context;

        public KhachHangsController(QuanLyNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var khachHangs = await _context.KhachHangs
                .Include(k => k.TenDangNhapNavigation)
                .ToListAsync();
            return View(khachHangs);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var khachHang = await _context.KhachHangs
                .Include(k => k.TenDangNhapNavigation)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SoDienThoai,TenDangNhap,MatKhau,HoTen,LoaiTaiKhoan,SoDu")] KhachHang khachHang)
        {
            ModelState.Remove("TenDangNhapNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem TenDangNhap đã tồn tại trong NguoiDung chưa
                    var nguoiDung = await _context.NguoiDungs
                        .FirstOrDefaultAsync(n => n.TenDangNhap == khachHang.TenDangNhap);

                    if (nguoiDung == null)
                    {
                        // Nếu chưa tồn tại, tạo mới bản ghi trong NguoiDung
                        nguoiDung = new NguoiDung
                        {
                            TenDangNhap = khachHang.TenDangNhap,
                            MatKhau = khachHang.MatKhau, // Lấy từ form
                            HoTen = khachHang.HoTen, // Lấy từ form
                            MaVaiTro = 3, // Giả định vai trò mặc định (ví dụ: Khách hàng, bạn có thể điều chỉnh)
                            NgayTao = DateTime.Now
                        };
                        _context.NguoiDungs.Add(nguoiDung);
                        await _context.SaveChangesAsync();
                    }

                    // Tạo khách hàng
                    khachHang.NgayTao = DateTime.Now;
                    _context.Add(khachHang);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.InnerException?.Message ?? ex.Message);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                ModelState.AddModelError("", "Dữ liệu không hợp lệ: " + string.Join("; ", errors));
            }
            return View(khachHang);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhachHang,SoDienThoai,TenDangNhap,MatKhau,HoTen,LoaiTaiKhoan,SoDu,NgayTao")] KhachHang khachHang)
        {
            if (id != khachHang.MaKhachHang)
            {
                return NotFound();
            }

            ModelState.Remove("TenDangNhapNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem TenDangNhap đã tồn tại trong NguoiDung chưa
                    var nguoiDung = await _context.NguoiDungs
                        .FirstOrDefaultAsync(n => n.TenDangNhap == khachHang.TenDangNhap);

                    if (nguoiDung == null)
                    {
                        // Nếu chưa tồn tại, tạo mới bản ghi trong NguoiDung
                        nguoiDung = new NguoiDung
                        {
                            TenDangNhap = khachHang.TenDangNhap,
                            MatKhau = khachHang.MatKhau,
                            HoTen = khachHang.HoTen,
                            MaVaiTro = 3, // Giả định vai trò mặc định
                            NgayTao = DateTime.Now
                        };
                        _context.NguoiDungs.Add(nguoiDung);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Cập nhật thông tin NguoiDung nếu cần
                        nguoiDung.MatKhau = khachHang.MatKhau;
                        nguoiDung.HoTen = khachHang.HoTen;
                        _context.Update(nguoiDung);
                        await _context.SaveChangesAsync();
                    }

                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật khách hàng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is DbUpdateConcurrencyException)
                    {
                        if (!KhachHangExists(khachHang.MaKhachHang))
                        {
                            return NotFound();
                        }
                        ModelState.AddModelError("", "Xung đột đồng thời: Dữ liệu đã bị thay đổi bởi người khác. Vui lòng thử lại.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.InnerException?.Message ?? ex.Message);
                    }
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                ModelState.AddModelError("", "Dữ liệu không hợp lệ: " + string.Join("; ", errors));
            }
            return View(khachHang);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var khachHang = await _context.KhachHangs
                .Include(k => k.TenDangNhapNavigation)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHang = await _context.KhachHangs
                .Include(k => k.DonHangs)
                .Include(k => k.KhachHangNhiemVus)
                .Include(k => k.LichSuNapTienMaKhachHangNavigations)
                .Include(k => k.LichSuNapTienTenDangNhapNavigations)
                .Include(k => k.PhanThuongDaNhans)
                .Include(k => k.PhienChois)
                .Include(k => k.ThongBaos)
                .FirstOrDefaultAsync(k => k.MaKhachHang == id);

            if (khachHang == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng để xóa.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                if (khachHang.DonHangs.Any())
                {
                    _context.DonHangs.RemoveRange(khachHang.DonHangs);
                }
                if (khachHang.KhachHangNhiemVus.Any())
                {
                    _context.KhachHangNhiemVus.RemoveRange(khachHang.KhachHangNhiemVus);
                }
                if (khachHang.LichSuNapTienMaKhachHangNavigations.Any())
                {
                    _context.LichSuNapTiens.RemoveRange(khachHang.LichSuNapTienMaKhachHangNavigations);
                }
                if (khachHang.LichSuNapTienTenDangNhapNavigations.Any())
                {
                    _context.LichSuNapTiens.RemoveRange(khachHang.LichSuNapTienTenDangNhapNavigations);
                }
                if (khachHang.PhanThuongDaNhans.Any())
                {
                    _context.PhanThuongDaNhans.RemoveRange(khachHang.PhanThuongDaNhans);
                }
                if (khachHang.PhienChois.Any())
                {
                    _context.PhienChois.RemoveRange(khachHang.PhienChois);
                }
                if (khachHang.ThongBaos.Any())
                {
                    _context.ThongBaos.RemoveRange(khachHang.ThongBaos);
                }

                _context.KhachHangs.Remove(khachHang);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Xóa khách hàng thành công!";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa dữ liệu: " + ex.InnerException?.Message ?? ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.MaKhachHang == id);
        }
    }
}