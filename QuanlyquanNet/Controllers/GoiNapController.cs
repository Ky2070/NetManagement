using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    public class GoiNapController : Controller
    {
        private readonly QuanLyNetContext _context;

        public GoiNapController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: GoiNap
        public async Task<IActionResult> Index()
        {
            var goiNaps = await _context.GoiNaps
                .Include(g => g.MaKhuyenMaiNavigation) // Liên kết với bảng KhuyenMai
                .ToListAsync();
            return View(goiNaps);
        }

        // GET: GoiNap/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goiNap = await _context.GoiNaps
                .Include(g => g.MaKhuyenMaiNavigation)
                .FirstOrDefaultAsync(m => m.MaGoiNap == id);
            if (goiNap == null)
            {
                return NotFound();
            }

            return View(goiNap);
        }

        // GET: GoiNap/Create
        public IActionResult Create()
        {
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai");
            return View();
        }

        // POST: GoiNap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenGoi,SoTien,MaKhuyenMai")] GoiNap goiNap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goiNap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", goiNap.MaKhuyenMai);
            return View(goiNap);
        }

        // GET: GoiNap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goiNap = await _context.GoiNaps.FindAsync(id);
            if (goiNap == null)
            {
                return NotFound();
            }
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", goiNap.MaKhuyenMai);
            return View(goiNap);
        }

        // POST: GoiNap/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGoiNap,TenGoi,SoTien,MaKhuyenMai,NgayTao")] GoiNap goiNap)
        {
            if (id != goiNap.MaGoiNap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goiNap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoiNapExists(goiNap.MaGoiNap))
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
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", goiNap.MaKhuyenMai);
            return View(goiNap);
        }

        // GET: GoiNap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goiNap = await _context.GoiNaps
                .Include(g => g.MaKhuyenMaiNavigation)
                .FirstOrDefaultAsync(m => m.MaGoiNap == id);
            if (goiNap == null)
            {
                return NotFound();
            }

            return View(goiNap);
        }

        // POST: GoiNap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goiNap = await _context.GoiNaps.FindAsync(id);
            _context.GoiNaps.Remove(goiNap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoiNapExists(int id)
        {
            return _context.GoiNaps.Any(e => e.MaGoiNap == id);
        }
    }
}
