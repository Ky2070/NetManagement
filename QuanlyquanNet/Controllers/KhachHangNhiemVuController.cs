using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    [Route("[controller]")]
    public class KhachHangNhiemVuController : Controller
    {
        private readonly QuanLyNetContext _context;

        public KhachHangNhiemVuController(QuanLyNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Danh sách khách hàng - nhiệm vụ";
            var khachHangNhiemVus = await _context.KhachHangNhiemVus
                .Include(k => k.MaKhachHangNavigation)
                .Include(k => k.MaNhiemVuNavigation)
                .ToListAsync();
            return View(khachHangNhiemVus);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(int? maKhachHang, int? maNhiemVu)
        {
            if (maKhachHang == null || maNhiemVu == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Chi tiết khách hàng - nhiệm vụ";
            var khachHangNhiemVu = await _context.KhachHangNhiemVus
                .Include(k => k.MaKhachHangNavigation)
                .Include(k => k.MaNhiemVuNavigation)
                .FirstOrDefaultAsync(m => m.MaKhachHang == maKhachHang && m.MaNhiemVu == maNhiemVu);
            if (khachHangNhiemVu == null)
            {
                return NotFound();
            }
            return View(khachHangNhiemVu);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Thêm khách hàng - nhiệm vụ mới";
            var khachHangs = _context.KhachHangs.ToList();
            var nhiemVus = _context.NhiemVus.ToList();
            if (!khachHangs.Any())
            {
                ModelState.AddModelError("", "Không có khách hàng nào trong cơ sở dữ liệu. Vui lòng thêm khách hàng trước.");
            }
            if (!nhiemVus.Any())
            {
                ModelState.AddModelError("", "Không có nhiệm vụ nào trong cơ sở dữ liệu. Vui lòng thêm nhiệm vụ trước.");
            }
            ViewData["MaKhachHang"] = new SelectList(khachHangs, "MaKhachHang", "HoTen");
            ViewData["MaNhiemVu"] = new SelectList(nhiemVus, "MaNhiemVu", "TenNhiemVu");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,MaNhiemVu,TrangThai,ThoiGianHoanThanh,DiemNhanDuoc,NgayThamGia")] KhachHangNhiemVu khachHangNhiemVu)
        {
            ViewData["Title"] = "Thêm khách hàng - nhiệm vụ mới";
            ModelState.Remove("MaKhachHangNavigation");
            ModelState.Remove("MaNhiemVuNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    khachHangNhiemVu.NgayThamGia = DateTime.Now;
                    _context.Add(khachHangNhiemVu);
                    await _context.SaveChangesAsync();
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
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", khachHangNhiemVu.MaKhachHang);
            ViewData["MaNhiemVu"] = new SelectList(_context.NhiemVus, "MaNhiemVu", "TenNhiemVu", khachHangNhiemVu.MaNhiemVu);
            return View(khachHangNhiemVu);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int? maKhachHang, int? maNhiemVu)
        {
            ViewData["Title"] = "Sửa khách hàng - nhiệm vụ";
            if (maKhachHang == null || maNhiemVu == null)
            {
                return NotFound();
            }

            var khachHangNhiemVu = await _context.KhachHangNhiemVus.FindAsync(maKhachHang, maNhiemVu);
            if (khachHangNhiemVu == null)
            {
                return NotFound();
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", khachHangNhiemVu.MaKhachHang);
            ViewData["MaNhiemVu"] = new SelectList(_context.NhiemVus, "MaNhiemVu", "TenNhiemVu", khachHangNhiemVu.MaNhiemVu);
            return View(khachHangNhiemVu);
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int maKhachHang, int maNhiemVu, [Bind("MaKhachHang,MaNhiemVu,TrangThai,ThoiGianHoanThanh,DiemNhanDuoc,NgayThamGia")] KhachHangNhiemVu khachHangNhiemVu)
        {
            ViewData["Title"] = "Sửa khách hàng - nhiệm vụ";
            if (maKhachHang != khachHangNhiemVu.MaKhachHang || maNhiemVu != khachHangNhiemVu.MaNhiemVu)
            {
                return NotFound();
            }

            ModelState.Remove("MaKhachHangNavigation");
            ModelState.Remove("MaNhiemVuNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHangNhiemVu);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangNhiemVuExists(khachHangNhiemVu.MaKhachHang, khachHangNhiemVu.MaNhiemVu))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
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
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", khachHangNhiemVu.MaKhachHang);
            ViewData["MaNhiemVu"] = new SelectList(_context.NhiemVus, "MaNhiemVu", "TenNhiemVu", khachHangNhiemVu.MaNhiemVu);
            return View(khachHangNhiemVu);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int? maKhachHang, int? maNhiemVu)
        {
            ViewData["Title"] = "Xóa khách hàng - nhiệm vụ";
            if (maKhachHang == null || maNhiemVu == null)
            {
                return NotFound();
            }

            var khachHangNhiemVu = await _context.KhachHangNhiemVus
                .Include(k => k.MaKhachHangNavigation)
                .Include(k => k.MaNhiemVuNavigation)
                .FirstOrDefaultAsync(m => m.MaKhachHang == maKhachHang && m.MaNhiemVu == maNhiemVu);
            if (khachHangNhiemVu == null)
            {
                return NotFound();
            }
            return View(khachHangNhiemVu);
        }

        [HttpPost("Delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int maKhachHang, int maNhiemVu)
        {
            ViewData["Title"] = "Xóa khách hàng - nhiệm vụ";
            var khachHangNhiemVu = await _context.KhachHangNhiemVus.FindAsync(maKhachHang, maNhiemVu);
            if (khachHangNhiemVu != null)
            {
                try
                {
                    _context.KhachHangNhiemVus.Remove(khachHangNhiemVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Lỗi khi xóa dữ liệu: " + ex.InnerException?.Message ?? ex.Message);
                    return View(khachHangNhiemVu);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangNhiemVuExists(int maKhachHang, int maNhiemVu)
        {
            return _context.KhachHangNhiemVus.Any(e => e.MaKhachHang == maKhachHang && e.MaNhiemVu == maNhiemVu);
        }
    }
}