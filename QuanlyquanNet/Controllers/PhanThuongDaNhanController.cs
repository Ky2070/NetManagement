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
    public class PhanThuongDaNhanController : Controller
    {
        private readonly QuanLyNetContext _context;

        public PhanThuongDaNhanController(QuanLyNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var phanThuongDaNhans = await _context.PhanThuongDaNhans
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.MaPhuThuongNavigation)
                .ToListAsync();
            return View(phanThuongDaNhans);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phanThuongDaNhan = await _context.PhanThuongDaNhans
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.MaPhuThuongNavigation)
                .FirstOrDefaultAsync(m => m.MaPhanThuong == id);
            if (phanThuongDaNhan == null)
            {
                return NotFound();
            }

            return View(phanThuongDaNhan);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var khachHangs = _context.KhachHangs.ToList();
            var phuThuongs = _context.PhuThuongs.ToList();

            if (!khachHangs.Any())
            {
                ModelState.AddModelError("", "Không có khách hàng nào trong cơ sở dữ liệu. Vui lòng thêm khách hàng trước.");
            }
            if (!phuThuongs.Any())
            {
                ModelState.AddModelError("", "Không có phần thưởng nào trong cơ sở dữ liệu. Vui lòng thêm phần thưởng trước.");
            }

            ViewData["MaKhachHang"] = new SelectList(khachHangs, "MaKhachHang", "HoTen");
            ViewData["MaPhuThuong"] = new SelectList(phuThuongs, "MaPhuThuong", "TenPhuThuong");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,MaPhuThuong,NgayNhan")] PhanThuongDaNhan phanThuongDaNhan)
        {
            ModelState.Remove("MaKhachHangNavigation");
            ModelState.Remove("MaPhuThuongNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    if (phanThuongDaNhan.NgayNhan == null)
                    {
                        phanThuongDaNhan.NgayNhan = DateTime.Now;
                    }
                    _context.Add(phanThuongDaNhan);
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

            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", phanThuongDaNhan.MaKhachHang);
            ViewData["MaPhuThuong"] = new SelectList(_context.PhuThuongs, "MaPhuThuong", "TenPhuThuong", phanThuongDaNhan.MaPhuThuong);
            return View(phanThuongDaNhan);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phanThuongDaNhan = await _context.PhanThuongDaNhans.FindAsync(id);
            if (phanThuongDaNhan == null)
            {
                return NotFound();
            }

            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", phanThuongDaNhan.MaKhachHang);
            ViewData["MaPhuThuong"] = new SelectList(_context.PhuThuongs, "MaPhuThuong", "TenPhuThuong", phanThuongDaNhan.MaPhuThuong);
            return View(phanThuongDaNhan);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhanThuong,MaKhachHang,MaPhuThuong,NgayNhan")] PhanThuongDaNhan phanThuongDaNhan)
        {
            if (id != phanThuongDaNhan.MaPhanThuong)
            {
                return NotFound();
            }

            ModelState.Remove("MaKhachHangNavigation");
            ModelState.Remove("MaPhuThuongNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phanThuongDaNhan);
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

            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", phanThuongDaNhan.MaKhachHang);
            ViewData["MaPhuThuong"] = new SelectList(_context.PhuThuongs, "MaPhuThuong", "TenPhuThuong", phanThuongDaNhan.MaPhuThuong);
            return View(phanThuongDaNhan);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phanThuongDaNhan = await _context.PhanThuongDaNhans
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.MaPhuThuongNavigation)
                .FirstOrDefaultAsync(m => m.MaPhanThuong == id);
            if (phanThuongDaNhan == null)
            {
                return NotFound();
            }

            return View(phanThuongDaNhan);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phanThuongDaNhan = await _context.PhanThuongDaNhans.FindAsync(id);
            if (phanThuongDaNhan != null)
            {
                try
                {
                    _context.PhanThuongDaNhans.Remove(phanThuongDaNhan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Không thể xóa phần thưởng vì có dữ liệu liên quan: " + ex.InnerException?.Message ?? ex.Message);
                    return View("Delete", phanThuongDaNhan);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PhanThuongDaNhanExists(int id)
        {
            return _context.PhanThuongDaNhans.Any(e => e.MaPhanThuong == id);
        }
    }
}