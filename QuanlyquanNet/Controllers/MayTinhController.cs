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
    public class MayTinhController : Controller
    {
        private readonly QuanLyNetContext _context;

        public MayTinhController(QuanLyNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var mayTinhs = await _context.MayTinhs
                .Include(m => m.MaKhuVucNavigation)
                .ToListAsync();
            return View(mayTinhs);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var mayTinh = await _context.MayTinhs
                .Include(m => m.MaKhuVucNavigation)
                .FirstOrDefaultAsync(m => m.MaMayTinh == id);
            if (mayTinh == null)
            {
                return NotFound();
            }
            return View(mayTinh);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var khuVucs = _context.KhuVucs.ToList();
            if (!khuVucs.Any())
            {
                ModelState.AddModelError("", "Không có khu vực nào trong cơ sở dữ liệu. Vui lòng thêm khu vực trước.");
            }
            ViewData["MaKhuVuc"] = new SelectList(khuVucs, "MaKhuVuc", "TenKhuVuc");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhuVuc,Stt,TenMayTinh,TrangThai")] MayTinh mayTinh)
        {
            ModelState.Remove("MaKhuVucNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    mayTinh.NgayTao = DateTime.Now;
                    _context.Add(mayTinh);
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
            ViewData["MaKhuVuc"] = new SelectList(_context.KhuVucs, "MaKhuVuc", "TenKhuVuc", mayTinh.MaKhuVuc);
            return View(mayTinh);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var mayTinh = await _context.MayTinhs.FindAsync(id);
            if (mayTinh == null)
            {
                return NotFound();
            }
            ViewData["MaKhuVuc"] = new SelectList(_context.KhuVucs, "MaKhuVuc", "TenKhuVuc", mayTinh.MaKhuVuc);
            return View(mayTinh);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaMayTinh,MaKhuVuc,Stt,TenMayTinh,TrangThai,NgayTao")] MayTinh mayTinh)
        {
            if (id != mayTinh.MaMayTinh)
            {
                return NotFound();
            }

            ModelState.Remove("MaKhuVucNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mayTinh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MayTinhExists(mayTinh.MaMayTinh))
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
            ViewData["MaKhuVuc"] = new SelectList(_context.KhuVucs, "MaKhuVuc", "TenKhuVuc", mayTinh.MaKhuVuc);
            return View(mayTinh);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mayTinh = await _context.MayTinhs
                .Include(m => m.MaKhuVucNavigation)
                .FirstOrDefaultAsync(m => m.MaMayTinh == id);
            if (mayTinh == null)
            {
                return NotFound();
            }
            return View(mayTinh);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mayTinh = await _context.MayTinhs.FindAsync(id);
            if (mayTinh != null)
            {
                _context.MayTinhs.Remove(mayTinh);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MayTinhExists(int id)
        {
            return _context.MayTinhs.Any(e => e.MaMayTinh == id);
        }
    }
}