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
    public class PhienChoisController : Controller
    {
        private readonly QuanLyNetContext _context;

        public PhienChoisController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: PhienChois
        public async Task<IActionResult> Index()
        {
            var quanLyNetContext = _context.PhienChois.Include(p => p.MaKhachHangNavigation).Include(p => p.MaMayTinhNavigation);
            return View(await quanLyNetContext.ToListAsync());
        }

        // GET: PhienChois/Details/5
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

        // GET: PhienChois/Create
        public IActionResult Create()
        {
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap");
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "MaMayTinh");
            return View();
        }

        // POST: PhienChois/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhienChoi,MaKhachHang,MaMayTinh,ThongTinViTri,ThoiGianBatDau,ThoiGianKetThuc,TongChiPhi,TrangThai")] PhienChoi phienChoi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phienChoi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap", phienChoi.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "MaMayTinh", phienChoi.MaMayTinh);
            return View(phienChoi);
        }

        // GET: PhienChois/Edit/5
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
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap", phienChoi.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "MaMayTinh", phienChoi.MaMayTinh);
            return View(phienChoi);
        }

        // POST: PhienChois/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhienChoi,MaKhachHang,MaMayTinh,ThongTinViTri,ThoiGianBatDau,ThoiGianKetThuc,TongChiPhi,TrangThai")] PhienChoi phienChoi)
        {
            if (id != phienChoi.MaPhienChoi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phienChoi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhienChoiExists(phienChoi.MaPhienChoi))
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
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap", phienChoi.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "MaMayTinh", phienChoi.MaMayTinh);
            return View(phienChoi);
        }

        // GET: PhienChois/Delete/5
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

        // POST: PhienChois/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phienChoi = await _context.PhienChois.FindAsync(id);
            if (phienChoi != null)
            {
                _context.PhienChois.Remove(phienChoi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhienChoiExists(int id)
        {
            return _context.PhienChois.Any(e => e.MaPhienChoi == id);
        }
    }
}
