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
    public class DichVuController : Controller
    {
        private readonly QuanLyNetContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DichVuController(QuanLyNetContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: DichVu
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var dichVus = await _context.DichVus.ToListAsync();
            return View(dichVus);
        }

        // GET: DichVu/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var dichVu = await _context.DichVus
                .FirstOrDefaultAsync(dv => dv.MaDichVu == id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        // GET: DichVu/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DichVu/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenDichVu,Gia,LoaiDichVu")] DichVu dichVu, IFormFile hinhAnhFile)
        {
            if (ModelState.IsValid)
            {
                // Xử lý tải lên hình ảnh
                if (hinhAnhFile != null && hinhAnhFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(hinhAnhFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận tệp JPG hoặc PNG.");
                        return View(dichVu);
                    }

                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnhFile.CopyToAsync(stream);
                    }
                    dichVu.HinhAnh = "/images/" + fileName;
                }

                dichVu.NgayTao = DateTime.Now;
                _context.Add(dichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dichVu);
        }

        // GET: DichVu/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        // POST: DichVu/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDichVu,TenDichVu,Gia,LoaiDichVu,HinhAnh,NgayTao")] DichVu dichVu, IFormFile hinhAnhFile)
        {
            if (id != dichVu.MaDichVu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý tải lên hình ảnh mới (nếu có)
                    if (hinhAnhFile != null && hinhAnhFile.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                        var fileExtension = Path.GetExtension(hinhAnhFile.FileName).ToLower();
                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận tệp JPG hoặc PNG.");
                            return View(dichVu);
                        }

                        // Xóa hình ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(dichVu.HinhAnh))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, dichVu.HinhAnh.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Lưu hình ảnh mới
                        var fileName = Guid.NewGuid().ToString() + fileExtension;
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await hinhAnhFile.CopyToAsync(stream);
                        }
                        dichVu.HinhAnh = "/images/" + fileName;
                    }

                    _context.Update(dichVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DichVuExists(dichVu.MaDichVu))
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
            return View(dichVu);
        }

        // GET: DichVu/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dichVu = await _context.DichVus
                .FirstOrDefaultAsync(dv => dv.MaDichVu == id);
            if (dichVu == null)
            {
                return NotFound();
            }
            return View(dichVu);
        }

        // POST: DichVu/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu != null)
            {
                // Xóa hình ảnh nếu có
                if (!string.IsNullOrEmpty(dichVu.HinhAnh))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, dichVu.HinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.DichVus.Remove(dichVu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DichVuExists(int id)
        {
            return _context.DichVus.Any(e => e.MaDichVu == id);
        }
    }
}
