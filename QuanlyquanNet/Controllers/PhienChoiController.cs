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
    public class PhienChoiController : Controller
    {
        private readonly QuanLyNetContext _context;

        public PhienChoiController(QuanLyNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var phienChois = await _context.PhienChois
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.MaMayTinhNavigation)
                .ToListAsync();
            return View(phienChois);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phienChoi = await _context.PhienChois
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.MaMayTinhNavigation)
                .FirstOrDefaultAsync(m => m.MaPhienChoi == id);
            if (phienChoi == null)
            {
                return NotFound();
            }

            return View(phienChoi);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen");
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "TenMayTinh");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,MaMayTinh,ThongTinViTri,ThoiGianBatDau,ThoiGianKetThuc,TongChiPhi,TrangThai")] PhienChoi phienChoi)
        {
            ModelState.Remove("MaKhachHangNavigation");
            ModelState.Remove("MaMayTinhNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    if (phienChoi.ThoiGianBatDau == null)
                    {
                        phienChoi.ThoiGianBatDau = DateTime.Now;
                    }
                    if (string.IsNullOrEmpty(phienChoi.TrangThai))
                    {
                        phienChoi.TrangThai = "Đang chơi";
                    }
                    _context.Add(phienChoi);
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

            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", phienChoi.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "TenMayTinh", phienChoi.MaMayTinh);
            return View(phienChoi);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phienChoi = await _context.PhienChois.FindAsync(id);
            if (phienChoi == null)
            {
                return NotFound();
            }

            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", phienChoi.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "TenMayTinh", phienChoi.MaMayTinh);
            return View(phienChoi);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhienChoi,MaKhachHang,MaMayTinh,ThongTinViTri,ThoiGianBatDau,ThoiGianKetThuc,TongChiPhi,TrangThai")] PhienChoi phienChoi)
        {
            if (id != phienChoi.MaPhienChoi)
            {
                return NotFound();
            }

            ModelState.Remove("MaKhachHangNavigation");
            ModelState.Remove("MaMayTinhNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phienChoi);
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

            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", phienChoi.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "TenMayTinh", phienChoi.MaMayTinh);
            return View(phienChoi);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phienChoi = await _context.PhienChois
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.MaMayTinhNavigation)
                .FirstOrDefaultAsync(m => m.MaPhienChoi == id);
            if (phienChoi == null)
            {
                return NotFound();
            }

            return View(phienChoi);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phienChoi = await _context.PhienChois.FindAsync(id);
            if (phienChoi != null)
            {
                try
                {
                    _context.PhienChois.Remove(phienChoi);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Không thể xóa phiên chơi vì có dữ liệu liên quan: " + ex.InnerException?.Message ?? ex.Message);
                    return View("Delete", phienChoi);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PhienChoiExists(int id)
        {
            return _context.PhienChois.Any(e => e.MaPhienChoi == id);
        }
    }
}