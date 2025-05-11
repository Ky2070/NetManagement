using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    [Route("[controller]")]
    public class NhiemVuController : Controller
    {
        private readonly QuanLyNetContext _context;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/nhiemvu");

        public NhiemVuController(QuanLyNetContext context)
        {
            _context = context;
            // Tạo thư mục lưu hình ảnh nếu chưa tồn tại
            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(_imagePath);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var nhiemVus = await _context.NhiemVus.ToListAsync();
            return View(nhiemVus);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var nhiemVu = await _context.NhiemVus.FirstOrDefaultAsync(n => n.MaNhiemVu == id);
            if (nhiemVu == null)
            {
                return NotFound();
            }
            return View(nhiemVu);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(new NhiemVu());
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhiemVu nhiemVu, IFormFile HinhAnh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (HinhAnh != null && HinhAnh.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg" };
                        var extension = Path.GetExtension(HinhAnh.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận file .jpg.");
                            return View(nhiemVu);
                        }

                        var fileName = Guid.NewGuid().ToString() + extension;
                        var filePath = Path.Combine(_imagePath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }
                        nhiemVu.HinhAnh = $"/images/nhiemvu/{fileName}";
                    }
                    nhiemVu.NgayTao = DateTime.Now;
                    _context.Add(nhiemVu);
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
            return View(nhiemVu ?? new NhiemVu());
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var nhiemVu = await _context.NhiemVus.FindAsync(id);
            if (nhiemVu == null)
            {
                return NotFound();
            }
            return View(nhiemVu);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NhiemVu nhiemVu, IFormFile HinhAnh)
        {
            if (id != nhiemVu.MaNhiemVu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (HinhAnh != null && HinhAnh.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg" };
                        var extension = Path.GetExtension(HinhAnh.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận file .jpg.");
                            return View(nhiemVu);
                        }

                        if (!string.IsNullOrEmpty(nhiemVu.HinhAnh))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", nhiemVu.HinhAnh.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        var fileName = Guid.NewGuid().ToString() + extension;
                        var filePath = Path.Combine(_imagePath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }
                        nhiemVu.HinhAnh = $"/images/nhiemvu/{fileName}";
                    }
                    _context.Update(nhiemVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhiemVuExists(nhiemVu.MaNhiemVu))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.InnerException?.Message ?? ex.Message);
                    return View(nhiemVu);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nhiemVu);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nhiemVu = await _context.NhiemVus.FirstOrDefaultAsync(n => n.MaNhiemVu == id);
            if (nhiemVu == null)
            {
                return NotFound();
            }
            return View(nhiemVu);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhiemVu = await _context.NhiemVus.FindAsync(id);
            if (nhiemVu != null)
            {
                if (!string.IsNullOrEmpty(nhiemVu.HinhAnh))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", nhiemVu.HinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.NhiemVus.Remove(nhiemVu);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    TempData["Error"] = "Không thể xóa nhiệm vụ do có dữ liệu liên quan.";
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NhiemVuExists(int id)
        {
            return _context.NhiemVus.Any(e => e.MaNhiemVu == id);
        }
    }
}