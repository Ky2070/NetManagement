using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    public class KhachHangNhiemVuController : Controller
    {
        private readonly QuanLyNetContext _context;

        public KhachHangNhiemVuController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: KhachHangNhiemVu
        public async Task<IActionResult> Index()
        {
            var khachHangNhiemVus = await _context.KhachHangNhiemVus
                .Include(k => k.MaKhachHangNavigation)
                .Include(k => k.MaNhiemVuNavigation)
                .ToListAsync();
            return View(khachHangNhiemVus);
        }

        // GET: KhachHangNhiemVu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHangNhiemVu = await _context.KhachHangNhiemVus
                .Include(k => k.MaKhachHangNavigation)
                .Include(k => k.MaNhiemVuNavigation)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id); // Assuming MaKhachHang is the identifier
            if (khachHangNhiemVu == null)
            {
                return NotFound();
            }

            return View(khachHangNhiemVu);
        }

        // GET: KhachHangNhiemVu/Create
        public IActionResult Create()
        {
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "Hoten");
            ViewData["MaNhiemVu"] = new SelectList(_context.NhiemVus, "MaNhiemVu", "TenNhiemVu");
            return View();
        }

        // POST: KhachHangNhiemVu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,MaNhiemVu,TrangThai,ThoiGianHoanThanh,DiemNhanDuoc,NgayThamGia")] KhachHangNhiemVu khachHangNhiemVu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHangNhiemVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", khachHangNhiemVu.MaKhachHang);
            ViewData["MaNhiemVu"] = new SelectList(_context.NhiemVus, "MaNhiemVu", "TenNhiemVu", khachHangNhiemVu.MaNhiemVu);
            return View(khachHangNhiemVu);
        }

        // GET: KhachHangNhiemVu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHangNhiemVu = await _context.KhachHangNhiemVus.FindAsync(id);
            if (khachHangNhiemVu == null)
            {
                return NotFound();
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", khachHangNhiemVu.MaKhachHang);
            ViewData["MaNhiemVu"] = new SelectList(_context.NhiemVus, "MaNhiemVu", "TenNhiemVu", khachHangNhiemVu.MaNhiemVu);
            return View(khachHangNhiemVu);
        }

        // POST: KhachHangNhiemVu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhachHang,MaNhiemVu,TrangThai,ThoiGianHoanThanh,DiemNhanDuoc,NgayThamGia")] KhachHangNhiemVu khachHangNhiemVu)
        {
            if (id != khachHangNhiemVu.MaKhachHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHangNhiemVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangNhiemVuExists(khachHangNhiemVu.MaKhachHang))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", khachHangNhiemVu.MaKhachHang);
            ViewData["MaNhiemVu"] = new SelectList(_context.NhiemVus, "MaNhiemVu", "TenNhiemVu", khachHangNhiemVu.MaNhiemVu);
            return View(khachHangNhiemVu);
        }

        // GET: KhachHangNhiemVu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHangNhiemVu = await _context.KhachHangNhiemVus
                .Include(k => k.MaKhachHangNavigation)
                .Include(k => k.MaNhiemVuNavigation)
                .FirstOrDefaultAsync(m => m.MaKhachHang == id);
            if (khachHangNhiemVu == null)
            {
                return NotFound();
            }

            return View(khachHangNhiemVu);
        }

        // POST: KhachHangNhiemVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khachHangNhiemVu = await _context.KhachHangNhiemVus.FindAsync(id);
            _context.KhachHangNhiemVus.Remove(khachHangNhiemVu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangNhiemVuExists(int id)
        {
            return _context.KhachHangNhiemVus.Any(e => e.MaKhachHang == id);
        }
    }
}
