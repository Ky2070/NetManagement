using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;

namespace QuanlyquanNet.Controllers
{
    public class LichSuNapTiensController : Controller
    {
        private readonly QuanLyNetContext _context;

        public LichSuNapTiensController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: LichSuNapTiens
        public async Task<IActionResult> Index()
        {
            var quanLyNetContext = _context.LichSuNapTiens.Include(l => l.MaGoiNapNavigation).Include(l => l.MaKhachHangNavigation).Include(l => l.TenDangNhapNavigation);
            return View(await quanLyNetContext.ToListAsync());
        }

        // GET: LichSuNapTiens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSuNapTien = await _context.LichSuNapTiens
                .Include(l => l.MaGoiNapNavigation)
                .Include(l => l.MaKhachHangNavigation)
                .Include(l => l.TenDangNhapNavigation)
                .FirstOrDefaultAsync(m => m.MaNapTien == id);
            if (lichSuNapTien == null)
            {
                return NotFound();
            }

            return View(lichSuNapTien);
        }

        // GET: LichSuNapTiens/Create
        public IActionResult Create()
        {
            ViewData["MaGoiNap"] = new SelectList(_context.GoiNaps, "MaGoiNap", "MaGoiNap");
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap");
            ViewData["TenDangNhap"] = new SelectList(_context.KhachHangs, "TenDangNhap", "TenDangNhap");
            return View();
        }

        // POST: LichSuNapTiens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNapTien,MaKhachHang,TenDangNhap,HoTen,MaGoiNap,SoTien,SoTienThucNhan,NgayNap,TrangThai")] LichSuNapTien lichSuNapTien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lichSuNapTien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaGoiNap"] = new SelectList(_context.GoiNaps, "MaGoiNap", "MaGoiNap", lichSuNapTien.MaGoiNap);
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap", lichSuNapTien.MaKhachHang);
            ViewData["TenDangNhap"] = new SelectList(_context.KhachHangs, "TenDangNhap", "TenDangNhap", lichSuNapTien.TenDangNhap);
            return View(lichSuNapTien);
        }

        // GET: LichSuNapTiens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSuNapTien = await _context.LichSuNapTiens.FindAsync(id);
            if (lichSuNapTien == null)
            {
                return NotFound();
            }
            ViewData["MaGoiNap"] = new SelectList(_context.GoiNaps, "MaGoiNap", "MaGoiNap", lichSuNapTien.MaGoiNap);
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap", lichSuNapTien.MaKhachHang);
            ViewData["TenDangNhap"] = new SelectList(_context.KhachHangs, "TenDangNhap", "TenDangNhap", lichSuNapTien.TenDangNhap);
            return View(lichSuNapTien);
        }

        // POST: LichSuNapTiens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNapTien,MaKhachHang,TenDangNhap,HoTen,MaGoiNap,SoTien,SoTienThucNhan,NgayNap,TrangThai")] LichSuNapTien lichSuNapTien)
        {
            if (id != lichSuNapTien.MaNapTien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lichSuNapTien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LichSuNapTienExists(lichSuNapTien.MaNapTien))
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
            ViewData["MaGoiNap"] = new SelectList(_context.GoiNaps, "MaGoiNap", "MaGoiNap", lichSuNapTien.MaGoiNap);
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap", lichSuNapTien.MaKhachHang);
            ViewData["TenDangNhap"] = new SelectList(_context.KhachHangs, "TenDangNhap", "TenDangNhap", lichSuNapTien.TenDangNhap);
            return View(lichSuNapTien);
        }

        // GET: LichSuNapTiens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSuNapTien = await _context.LichSuNapTiens
                .Include(l => l.MaGoiNapNavigation)
                .Include(l => l.MaKhachHangNavigation)
                .Include(l => l.TenDangNhapNavigation)
                .FirstOrDefaultAsync(m => m.MaNapTien == id);
            if (lichSuNapTien == null)
            {
                return NotFound();
            }

            return View(lichSuNapTien);
        }

        // POST: LichSuNapTiens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lichSuNapTien = await _context.LichSuNapTiens.FindAsync(id);
            if (lichSuNapTien != null)
            {
                _context.LichSuNapTiens.Remove(lichSuNapTien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LichSuNapTienExists(int id)
        {
            return _context.LichSuNapTiens.Any(e => e.MaNapTien == id);
        }
    }
}
