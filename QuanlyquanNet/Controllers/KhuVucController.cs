using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    public class KhuVucController : Controller
    {
        private readonly QuanLyNetContext _context;

        public KhuVucController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: Danh sách khu vực
        public async Task<IActionResult> Index()
        {
            var khuVucs = await _context.KhuVucs.ToListAsync();
            return View(khuVucs);
        }

        // GET: Form thêm khu vực
        public IActionResult Create()
        {
            return View();
        }

        // POST: Thêm khu vực
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhuVuc model, IFormFile? hinhAnh)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Handle image upload
                if (hinhAnh != null && hinhAnh.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(hinhAnh.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnh.CopyToAsync(stream);
                    }
                    model.HinhAnh = "/images/" + fileName;
                }

                _context.KhuVucs.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Khu vực đã được thêm thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi thêm khu vực. Vui lòng thử lại.";
                return View(model);
            }
        }

        // GET: Chi tiết khu vực
        public async Task<IActionResult> Details(int id)
        {
            var khuVuc = await _context.KhuVucs
                .Include(kv => kv.MayTinhs)
                .FirstOrDefaultAsync(kv => kv.MaKhuVuc == id);

            if (khuVuc == null)
                return NotFound();

            return View(khuVuc);
        }

        // GET: Form sửa
        public async Task<IActionResult> Edit(int id)
        {
            var khuVuc = await _context.KhuVucs.FindAsync(id);
            if (khuVuc == null)
                return NotFound();

            return View(khuVuc);
        }

        // POST: Cập nhật khu vực
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KhuVuc model, IFormFile? hinhAnh)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _context.KhuVucs.FindAsync(model.MaKhuVuc);
            if (existing == null)
                return NotFound();

            try
            {
                // Update fields
                existing.TenKhuVuc = model.TenKhuVuc;
                existing.MoTa = model.MoTa;

                // Handle image upload
                if (hinhAnh != null && hinhAnh.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(hinhAnh.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnh.CopyToAsync(stream);
                    }
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(existing.HinhAnh))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existing.HinhAnh.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }
                    existing.HinhAnh = "/images/" + fileName;
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Khu vực đã được cập nhật thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi cập nhật khu vực. Vui lòng thử lại.";
                return View(model);
            }
        }

        // GET: Xác nhận xoá
        public async Task<IActionResult> Delete(int id)
        {
            var khuVuc = await _context.KhuVucs.FindAsync(id);
            if (khuVuc == null)
                return NotFound();

            return View(khuVuc);
        }

        // POST: Xoá khu vực
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var khuVuc = await _context.KhuVucs.FindAsync(id);
                if (khuVuc == null)
                    return NotFound();

                // Optionally delete associated image
                if (!string.IsNullOrEmpty(khuVuc.HinhAnh))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", khuVuc.HinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }

                _context.KhuVucs.Remove(khuVuc);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Khu vực đã được xóa thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi xóa khu vực. Vui lòng thử lại.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}