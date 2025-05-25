using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using QuanlyquanNet.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    [Authorize(Roles = "Employee")]
    [Route("[controller]")]
    public class KhachHangsController : Controller
    {
        private readonly QuanLyNetContext _context;
        private readonly IWebHostEnvironment _env;

        public KhachHangsController(QuanLyNetContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

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
            var goiNapList = _context.GoiNaps
            .Select(g => new SelectListItem
                {
                    Value = g.MaGoiNap.ToString(),
                    Text = $"{g.TenGoi} - {g.SoTien:N0}đ"
                }).ToList();

            ViewBag.GoiNapList = goiNapList;

            var today = DateTime.Today;
            ViewBag.KhuyenMaiList = _context.KhuyenMais
                .Where(k => k.NgayBatDau.HasValue && k.NgayKetThuc.HasValue &&
                       k.NgayBatDau.Value <= today && k.NgayKetThuc.Value >= today)
                .Select(k => new SelectListItem
                {
                    Value = k.MaKhuyenMai.ToString(),
                    Text = $"{k.TenKhuyenMai} (+{k.PhanTramTang}%)"
                }).ToList();

            // Nếu cần toàn bộ chi tiết để render JSON:
            ViewData["AllGoiNap"] = _context.GoiNaps.ToList();
            ViewData["AllKhuyenMai"] = _context.KhuyenMais.ToList();
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
                        khachHang.NgayTao = DateTime.Now;
                        _context.Update(khachHang);
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

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult NapTien(int MaKhachHang, int MaGoiNap)
        {
            var khachHang = _context.KhachHangs.FirstOrDefault(k => k.MaKhachHang == MaKhachHang);
            var goiNap = _context.GoiNaps.Include(g => g.MaKhuyenMaiNavigation).FirstOrDefault(g => g.MaGoiNap == MaGoiNap);

            if (khachHang == null || goiNap == null)
                return NotFound();

            decimal soTien = goiNap.SoTien;
            decimal phanTramTang = goiNap.MaKhuyenMaiNavigation?.PhanTramTang ?? 0;
            decimal soTienThucNhan = soTien + (soTien * phanTramTang / 100);

            khachHang.SoDu = (khachHang.SoDu ?? 0) + soTienThucNhan;

            var lichSu = new LichSuNapTien
            {
                MaKhachHang = khachHang.MaKhachHang,
                TenDangNhap = khachHang.TenDangNhap,
                HoTen = khachHang.HoTen,
                MaGoiNap = goiNap.MaGoiNap,
                SoTien = soTien,
                SoTienThucNhan = soTienThucNhan,
                NgayNap = DateTime.Now,
                TrangThai = "Thành công"
            };

            _context.LichSuNapTiens.Add(lichSu);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Nạp {soTienThucNhan:N0}đ thành công cho {khachHang.HoTen}";
            return RedirectToAction("Details", new { id = MaKhachHang });
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