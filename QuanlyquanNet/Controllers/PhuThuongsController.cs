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
    public class PhuThuongsController : Controller
    {
        private readonly QuanLyNetContext _context;

        public PhuThuongsController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: PhuThuongs
        public async Task<IActionResult> Index()
        {
            var quanLyNetContext = _context.PhuThuongs.Include(p => p.MaVatPhamNavigation);
            return View(await quanLyNetContext.ToListAsync());
        }

        // GET: PhuThuongs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phuThuong = await _context.PhuThuongs
                .Include(p => p.MaVatPhamNavigation)
                .FirstOrDefaultAsync(m => m.MaPhuThuong == id);
            if (phuThuong == null)
            {
                return NotFound();
            }

            return View(phuThuong);
        }

        // GET: PhuThuongs/Create
        public IActionResult Create()
        {
            ViewData["MaVatPham"] = new SelectList(_context.VatPhams, "MaVatPham", "MaVatPham");
            return View();
        }

        // POST: PhuThuongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhuThuong,TenPhuThuong,LoaiPhuThuong,GiaTri,DiemCanDoi,MaVatPham,NgayTao")] PhuThuong phuThuong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phuThuong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaVatPham"] = new SelectList(_context.VatPhams, "MaVatPham", "MaVatPham", phuThuong.MaVatPham);
            return View(phuThuong);
        }

        // GET: PhuThuongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phuThuong = await _context.PhuThuongs.FindAsync(id);
            if (phuThuong == null)
            {
                return NotFound();
            }
            ViewData["MaVatPham"] = new SelectList(_context.VatPhams, "MaVatPham", "MaVatPham", phuThuong.MaVatPham);
            return View(phuThuong);
        }

        // POST: PhuThuongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhuThuong,TenPhuThuong,LoaiPhuThuong,GiaTri,DiemCanDoi,MaVatPham,NgayTao")] PhuThuong phuThuong)
        {
            if (id != phuThuong.MaPhuThuong)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phuThuong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhuThuongExists(phuThuong.MaPhuThuong))
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
            ViewData["MaVatPham"] = new SelectList(_context.VatPhams, "MaVatPham", "MaVatPham", phuThuong.MaVatPham);
            return View(phuThuong);
        }

        // GET: PhuThuongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phuThuong = await _context.PhuThuongs
                .Include(p => p.MaVatPhamNavigation)
                .FirstOrDefaultAsync(m => m.MaPhuThuong == id);
            if (phuThuong == null)
            {
                return NotFound();
            }

            return View(phuThuong);
        }

        // POST: PhuThuongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phuThuong = await _context.PhuThuongs.FindAsync(id);
            if (phuThuong != null)
            {
                _context.PhuThuongs.Remove(phuThuong);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhuThuongExists(int id)
        {
            return _context.PhuThuongs.Any(e => e.MaPhuThuong == id);
        }
    }
}
