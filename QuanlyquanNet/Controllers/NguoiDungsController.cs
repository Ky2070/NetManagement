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
    public class NguoiDungsController : Controller
    {
        private readonly QuanLyNetContext _context;

        public NguoiDungsController(QuanLyNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var nguoiDungs = await _context.NguoiDungs
                .Include(n => n.MaVaiTroNavigation)
                .ToListAsync();
            return View(nguoiDungs);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var nguoiDung = await _context.NguoiDungs
                .Include(n => n.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            return View(nguoiDung);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var vaiTros = _context.VaiTros.ToList();
            if (!vaiTros.Any())
            {
                ModelState.AddModelError("", "Không có vai trò nào trong cơ sở dữ liệu. Vui lòng thêm vai trò trước.");
            }
            ViewData["MaVaiTro"] = new SelectList(vaiTros, "MaVaiTro", "TenVaiTro");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenDangNhap,MatKhau,MaVaiTro,HoTen")] NguoiDung nguoiDung)
        {
            ModelState.Remove("MaVaiTroNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    nguoiDung.NgayTao = DateTime.Now;
                    _context.Add(nguoiDung);
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
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro", nguoiDung.MaVaiTro);
            return View(nguoiDung);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro", nguoiDung.MaVaiTro);
            return View(nguoiDung);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNguoiDung,TenDangNhap,MatKhau,MaVaiTro,HoTen,NgayTao")] NguoiDung nguoiDung)
        {
            if (id != nguoiDung.MaNguoiDung)
            {
                return NotFound();
            }

            ModelState.Remove("MaVaiTroNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguoiDung);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is DbUpdateConcurrencyException)
                    {
                        if (!NguoiDungExists(nguoiDung.MaNguoiDung))
                        {
                            return NotFound();
                        }
                        ModelState.AddModelError("", "Xung đột đồng thời: Dữ liệu đã bị thay đổi bởi người khác. Vui lòng thử lại.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.InnerException?.Message ?? ex.Message);
                    }
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                ModelState.AddModelError("", "Dữ liệu không hợp lệ: " + string.Join("; ", errors));
            }
            ViewData["MaVaiTro"] = new SelectList(_context.VaiTros, "MaVaiTro", "TenVaiTro", nguoiDung.MaVaiTro);
            return View(nguoiDung);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nguoiDung = await _context.NguoiDungs
                .Include(n => n.MaVaiTroNavigation)
                .FirstOrDefaultAsync(m => m.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }
            return View(nguoiDung);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nguoiDung = await _context.NguoiDungs
                .Include(nd => nd.LichSuDangNhaps)
                .FirstOrDefaultAsync(nd => nd.MaNguoiDung == id);

            if (nguoiDung == null)
                return RedirectToAction(nameof(Index));

            try
            {
                if (nguoiDung.LichSuDangNhaps.Any())
                {
                    _context.LichSuDangNhaps.RemoveRange(nguoiDung.LichSuDangNhaps);
                    await _context.SaveChangesAsync(); // BẮT BUỘC gọi SaveChanges trước
                }

                _context.NguoiDungs.Remove(nguoiDung);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Lỗi khi xóa dữ liệu: " + (ex.InnerException?.Message ?? ex.Message));
                return View(nguoiDung);
            }

            return RedirectToAction(nameof(Index));
        }



        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.MaNguoiDung == id);
        }
    }
}