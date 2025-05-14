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
    public class PhanThuongDaNhansController : Controller
    {
        private readonly QuanLyNetContext _context;

        public PhanThuongDaNhansController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: PhanThuongDaNhans
        public async Task<IActionResult> Index()
        {
            var quanLyNetContext = _context.PhanThuongDaNhans.Include(p => p.MaKhachHangNavigation).Include(p => p.MaPhuThuongNavigation);
            return View(await quanLyNetContext.ToListAsync());
        }

        // GET: PhanThuongDaNhans/Details/5
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

        // GET: PhanThuongDaNhans/Create
        public IActionResult Create()
        {
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap");
            ViewData["MaPhuThuong"] = new SelectList(_context.PhuThuongs, "MaPhuThuong", "MaPhuThuong");
            return View();
        }

        // POST: PhanThuongDaNhans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhanThuong,MaKhachHang,MaPhuThuong,NgayNhan")] PhanThuongDaNhan phanThuongDaNhan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phanThuongDaNhan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap", phanThuongDaNhan.MaKhachHang);
            ViewData["MaPhuThuong"] = new SelectList(_context.PhuThuongs, "MaPhuThuong", "MaPhuThuong", phanThuongDaNhan.MaPhuThuong);
            return View(phanThuongDaNhan);
        }

        // GET: PhanThuongDaNhans/Edit/5
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
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap", phanThuongDaNhan.MaKhachHang);
            ViewData["MaPhuThuong"] = new SelectList(_context.PhuThuongs, "MaPhuThuong", "MaPhuThuong", phanThuongDaNhan.MaPhuThuong);
            return View(phanThuongDaNhan);
        }

        // POST: PhanThuongDaNhans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhanThuong,MaKhachHang,MaPhuThuong,NgayNhan")] PhanThuongDaNhan phanThuongDaNhan)
        {
            if (id != phanThuongDaNhan.MaPhanThuong)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phanThuongDaNhan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhanThuongDaNhanExists(phanThuongDaNhan.MaPhanThuong))
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
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenDangNhap", phanThuongDaNhan.MaKhachHang);
            ViewData["MaPhuThuong"] = new SelectList(_context.PhuThuongs, "MaPhuThuong", "MaPhuThuong", phanThuongDaNhan.MaPhuThuong);
            return View(phanThuongDaNhan);
        }

        // GET: PhanThuongDaNhans/Delete/5
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

        // POST: PhanThuongDaNhans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phanThuongDaNhan = await _context.PhanThuongDaNhans.FindAsync(id);
            if (phanThuongDaNhan != null)
            {
                _context.PhanThuongDaNhans.Remove(phanThuongDaNhan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhanThuongDaNhanExists(int id)
        {
            return _context.PhanThuongDaNhans.Any(e => e.MaPhanThuong == id);
        }
    }
}
