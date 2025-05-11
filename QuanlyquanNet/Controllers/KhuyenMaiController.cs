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
    public class KhuyenMaiController : Controller
    {
        private readonly QuanLyNetContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public KhuyenMaiController(QuanLyNetContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var khuyenMais = await _context.KhuyenMais.ToListAsync();
            return View(khuyenMais);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var khuyenMai = await _context.KhuyenMais
                .FirstOrDefaultAsync(km => km.MaKhuyenMai == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }
            return View(khuyenMai);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenKhuyenMai,PhanTramTang,NgayBatDau,NgayKetThuc")] KhuyenMai khuyenMai, IFormFile HinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (khuyenMai.NgayKetThuc < khuyenMai.NgayBatDau)
                {
                    ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
                    return View(khuyenMai);
                }

                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(HinhAnh.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận tệp JPG hoặc PNG.");
                        return View(khuyenMai);
                    }

                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhAnh.CopyToAsync(stream);
                    }
                    khuyenMai.HinhAnh = "/images/" + fileName;
                }

                khuyenMai.NgayTao = DateTime.Now;
                _context.Add(khuyenMai);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khuyenMai);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                return NotFound();
            }
            return View(khuyenMai);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhuyenMai,TenKhuyenMai,PhanTramTang,NgayBatDau,NgayKetThuc,HinhAnh,NgayTao")] KhuyenMai khuyenMai, IFormFile HinhAnh)
        {
            if (id != khuyenMai.MaKhuyenMai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (khuyenMai.NgayKetThuc < khuyenMai.NgayBatDau)
                {
                    ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
                    return View(khuyenMai);
                }

                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(HinhAnh.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận tệp JPG hoặc PNG.");
                        return View(khuyenMai);
                    }

                    if (!string.IsNullOrEmpty(khuyenMai.HinhAnh))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, khuyenMai.HinhAnh.TrimStart('/'));
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
                    khuyenMai.HinhAnh = "/images/" + fileName;
                }

                _context.Update(khuyenMai);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khuyenMai);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var khuyenMai = await _context.KhuyenMais
                .FirstOrDefaultAsync(km => km.MaKhuyenMai == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }
            return View(khuyenMai);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai != null)
            {
                if (!string.IsNullOrEmpty(khuyenMai.HinhAnh))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, khuyenMai.HinhAnh.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

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