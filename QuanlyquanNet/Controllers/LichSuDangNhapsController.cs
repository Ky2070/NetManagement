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
    public class LichSuDangNhapsController : Controller
    {
        private readonly QuanLyNetContext _context;

        public LichSuDangNhapsController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: LichSuDangNhaps
        public async Task<IActionResult> Index()
        {
            var quanLyNetContext = _context.LichSuDangNhaps.Include(l => l.MaNguoiDungNavigation);
            return View(await quanLyNetContext.ToListAsync());
        }

        // GET: LichSuDangNhaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSuDangNhap = await _context.LichSuDangNhaps
                .Include(l => l.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(m => m.MaDangNhap == id);
            if (lichSuDangNhap == null)
            {
                return NotFound();
            }

            return View(lichSuDangNhap);
        }

        // GET: LichSuDangNhaps/Create
        public IActionResult Create()
        {
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "TenDangNhap");
            return View();
        }

        // POST: LichSuDangNhaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDangNhap,TenDangNhap,HoTen,MaNguoiDung,ThoiGianDangNhap")] LichSuDangNhap lichSuDangNhap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lichSuDangNhap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "TenDangNhap", lichSuDangNhap.MaNguoiDung);
            return View(lichSuDangNhap);
        }

        // GET: LichSuDangNhaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSuDangNhap = await _context.LichSuDangNhaps.FindAsync(id);
            if (lichSuDangNhap == null)
            {
                return NotFound();
            }
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "TenDangNhap", lichSuDangNhap.MaNguoiDung);
            return View(lichSuDangNhap);
        }

        // POST: LichSuDangNhaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDangNhap,TenDangNhap,HoTen,MaNguoiDung,ThoiGianDangNhap")] LichSuDangNhap lichSuDangNhap)
        {
            if (id != lichSuDangNhap.MaDangNhap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lichSuDangNhap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LichSuDangNhapExists(lichSuDangNhap.MaDangNhap))
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
            ViewData["MaNguoiDung"] = new SelectList(_context.NguoiDungs, "MaNguoiDung", "TenDangNhap", lichSuDangNhap.MaNguoiDung);
            return View(lichSuDangNhap);
        }

        // GET: LichSuDangNhaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSuDangNhap = await _context.LichSuDangNhaps
                .Include(l => l.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(m => m.MaDangNhap == id);
            if (lichSuDangNhap == null)
            {
                return NotFound();
            }

            return View(lichSuDangNhap);
        }

        // POST: LichSuDangNhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lichSuDangNhap = await _context.LichSuDangNhaps.FindAsync(id);
            if (lichSuDangNhap != null)
            {
                _context.LichSuDangNhaps.Remove(lichSuDangNhap);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LichSuDangNhapExists(int id)
        {
            return _context.LichSuDangNhaps.Any(e => e.MaDangNhap == id);
        }
    }
}
