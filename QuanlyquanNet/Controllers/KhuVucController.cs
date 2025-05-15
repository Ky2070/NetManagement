using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace QuanlyquanNet.Controllers
{
    [Route("[controller]")]
    public class KhuVucController : Controller
    {
        private readonly QuanLyNetContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public KhuVucController(QuanLyNetContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var khuVucs = await _context.KhuVucs.ToListAsync();
            return View(khuVucs);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var khuVuc = await _context.KhuVucs
                .FirstOrDefaultAsync(kv => kv.MaKhuVuc == id);
            if (khuVuc == null)
            {
                return NotFound();
            }
            return View(khuVuc);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenKhuVuc,MoTa,GiaMoiGio")] KhuVuc khuVuc, IFormFile HinhAnh)
        {
            if (ModelState.IsValid)
            {
                // Xử lý tải lên hình ảnh
                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(HinhAnh.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận tệp JPG hoặc PNG.");
                        return View(khuVuc);
                    }

                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhAnh.CopyToAsync(stream);
                    }
                    khuVuc.HinhAnh = "/images/" + fileName;
                }

                khuVuc.NgayTao = DateTime.Now;
                _context.Add(khuVuc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khuVuc);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var khuVuc = await _context.KhuVucs.FindAsync(id);
            if (khuVuc == null)
            {
                return NotFound();
            }
            return View(khuVuc);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhuVuc,TenKhuVuc,MoTa,GiaMoiGio,NgayTao")] KhuVuc khuVuc, IFormFile HinhAnh)
        {
            if (id != khuVuc.MaKhuVuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var khuVucCu = await _context.KhuVucs.FindAsync(id);
                    if (khuVucCu == null) return NotFound();

                    // Cập nhật các trường
                    khuVucCu.TenKhuVuc = khuVuc.TenKhuVuc;
                    khuVucCu.MoTa = khuVuc.MoTa;
                    khuVucCu.GiaMoiGio = khuVuc.GiaMoiGio;
                    khuVucCu.NgayTao = DateTime.Now;

                    // Xử lý hình ảnh
                    if (HinhAnh != null && HinhAnh.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var fileExtension = Path.GetExtension(HinhAnh.FileName).ToLower();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận tệp JPG hoặc PNG.");
                            return View(khuVuc);
                        }

                        if (!string.IsNullOrEmpty(khuVucCu.HinhAnh))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, khuVucCu.HinhAnh.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var fileName = Guid.NewGuid().ToString() + fileExtension;
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }

                        khuVucCu.HinhAnh = "/images/" + fileName;
                    }

                    _context.Update(khuVucCu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhuVucExists(khuVuc.MaKhuVuc))
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

            return View(khuVuc);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var khuVuc = await _context.KhuVucs.FindAsync(id);
            if (khuVuc == null)
            {
                return NotFound();
            }
            return View(khuVuc); // View hiển thị xác nhận
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khuVuc = await _context.KhuVucs.FindAsync(id);

            // Kiểm tra xem có máy tính nào đang thuộc khu vực này không
            var mayTinhTonTai = _context.MayTinhs.Any(mt => mt.MaKhuVuc == id);
            if (mayTinhTonTai)
            {
                ModelState.AddModelError("", "Không thể xóa khu vực vì còn máy tính thuộc khu vực này.");
                return View("Delete", khuVuc);
            }

            if (khuVuc == null)
            {
                return NotFound();
            }
            else {
                // Xóa hình ảnh nếu có
                if (!string.IsNullOrEmpty(khuVuc.HinhAnh))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, khuVuc.HinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.KhuVucs.Remove(khuVuc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KhuVucExists(int id)
        {
            return _context.KhuVucs.Any(e => e.MaKhuVuc == id);
        }
    }
}