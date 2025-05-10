using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;

namespace QuanlyquanNet.Controllers
{
    public class KhuyenMaiController : Controller
    {
        private readonly QuanLyNetContext _context;

        public KhuyenMaiController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: KhuyenMai
        public async Task<IActionResult> Index()
        {
            return View(await _context.KhuyenMais.ToListAsync());
        }

        // GET: KhuyenMai/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var khuyenMai = await _context.KhuyenMais.FirstOrDefaultAsync(m => m.MaKhuyenMai == id);
            if (khuyenMai == null) return NotFound();

            return View(khuyenMai);
        }

        // GET: KhuyenMai/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhuyenMai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhuyenMai,TenKhuyenMai,PhanTramGiamGia,NgayBatDau,NgayKetThuc,NgayTao")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khuyenMai);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khuyenMai);
        }

        // GET: KhuyenMai/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai == null) return NotFound();

            return View(khuyenMai);
        }

        // POST: KhuyenMai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhuyenMai,TenKhuyenMai,PhanTramGiamGia,NgayBatDau,NgayKetThuc,NgayTao")] KhuyenMai khuyenMai)
        {
            if (id != khuyenMai.MaKhuyenMai) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khuyenMai);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhuyenMaiExists(khuyenMai.MaKhuyenMai)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khuyenMai);
        }

        // GET: KhuyenMai/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var khuyenMai = await _context.KhuyenMais.FirstOrDefaultAsync(m => m.MaKhuyenMai == id);
            if (khuyenMai == null) return NotFound();

            return View(khuyenMai);
        }

        // POST: KhuyenMai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai != null)
            {
                _context.KhuyenMais.Remove(khuyenMai);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KhuyenMaiExists(int id)
        {
            return _context.KhuyenMais.Any(e => e.MaKhuyenMai == id);
        }
    }
}
