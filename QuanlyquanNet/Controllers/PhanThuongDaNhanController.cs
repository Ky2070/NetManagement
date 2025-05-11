using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    [Route("[controller]")]
    public class PhanThuongDaNhanController : Controller
    {
        private readonly QuanLyNetContext QuanLyNetContext;

        public PhanThuongDaNhanController(QuanLyNetContext context)
        {
            QuanLyNetContext = context;
        }

        // GET: PhanThuongDaNhan
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var phanThuongs = await QuanLyNetContext.PhanThuongDaNhans
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.MaPhuThuongNavigation)
                .ToListAsync();
            return View(phanThuongs);
        }

        // GET: PhanThuongDaNhan/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var phanThuong = await QuanLyNetContext.PhanThuongDaNhans
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.MaPhuThuongNavigation)
                .FirstOrDefaultAsync(p => p.MaPhanThuong == id);
            if (phanThuong == null)
            {
                return NotFound();
            }
            return View(phanThuong);
        }

        // GET: PhanThuongDaNhan/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.KhachHangs = await QuanLyNetContext.KhachHangs.ToListAsync();
            ViewBag.PhuThuongs = await QuanLyNetContext.PhuThuongs.ToListAsync();
            return View();
        }

        // POST: PhanThuongDaNhan/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,MaPhuThuong,NgayNhan")] PhanThuongDaNhan phanThuong)
        {
            Console.WriteLine($"Received: MaKhachHang={phanThuong.MaKhachHang}, MaPhuThuong={phanThuong.MaPhuThuong}, NgayNhan={phanThuong.NgayNhan}");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                Console.WriteLine("Validation errors: " + string.Join(", ", errors));
                ViewBag.KhachHangs = await QuanLyNetContext.KhachHangs.ToListAsync();
                ViewBag.PhuThuongs = await QuanLyNetContext.PhuThuongs.ToListAsync();
                return View(phanThuong);
            }

            try
            {
                var khachHangExists = await QuanLyNetContext.KhachHangs.AnyAsync(kh => kh.MaKhachHang == phanThuong.MaKhachHang);
                if (!khachHangExists)
                {
                    Console.WriteLine("Invalid MaKhachHang: " + phanThuong.MaKhachHang);
                    ModelState.AddModelError("MaKhachHang", "Mã khách hàng không tồn tại.");
                    ViewBag.KhachHangs = await QuanLyNetContext.KhachHangs.ToListAsync();
                    ViewBag.PhuThuongs = await QuanLyNetContext.PhuThuongs.ToListAsync();
                    return View(phanThuong);
                }

                var phuThuongExists = await QuanLyNetContext.PhuThuongs.AnyAsync(pt => pt.MaPhuThuong == phanThuong.MaPhuThuong);
                if (!phuThuongExists)
                {
                    Console.WriteLine("Invalid MaPhuThuong: " + phanThuong.MaPhuThuong);
                    ModelState.AddModelError("MaPhuThuong", "Mã phần thưởng không tồn tại.");
                    ViewBag.KhachHangs = await QuanLyNetContext.KhachHangs.ToListAsync();
                    ViewBag.PhuThuongs = await QuanLyNetContext.PhuThuongs.ToListAsync();
                    return View(phanThuong);
                }

                var maxId = await QuanLyNetContext.PhanThuongDaNhans.MaxAsync(p => (int?)p.MaPhanThuong) ?? 0;
                phanThuong.MaPhanThuong = maxId + 1;

                Console.WriteLine("Saving to database...");
                QuanLyNetContext.Add(phanThuong);
                await QuanLyNetContext.SaveChangesAsync();
                Console.WriteLine("Saved successfully, redirecting to Index");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo phần thưởng: " + ex.Message);
                ViewBag.KhachHangs = await QuanLyNetContext.KhachHangs.ToListAsync();
                ViewBag.PhuThuongs = await QuanLyNetContext.PhuThuongs.ToListAsync();
                return View(phanThuong);
            }
        }

        // GET: PhanThuongDaNhan/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var phanThuong = await QuanLyNetContext.PhanThuongDaNhans.FindAsync(id);
            if (phanThuong == null)
            {
                return NotFound();
            }
            ViewBag.KhachHangs = await QuanLyNetContext.KhachHangs.ToListAsync();
            ViewBag.PhuThuongs = await QuanLyNetContext.PhuThuongs.ToListAsync();
            return View(phanThuong);
        }

        // POST: PhanThuongDaNhan/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhanThuong,MaKhachHang,MaPhuThuong,NgayNhan")] PhanThuongDaNhan phanThuong)
        {
            if (id != phanThuong.MaPhanThuong)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var khachHangExists = await QuanLyNetContext.KhachHangs.AnyAsync(kh => kh.MaKhachHang == phanThuong.MaKhachHang);
                    if (!khachHangExists)
                    {
                        ModelState.AddModelError("MaKhachHang", "Mã khách hàng không tồn tại.");
                        ViewBag.KhachHangs = await QuanLyNetContext.KhachHangs.ToListAsync();
                        ViewBag.PhuThuongs = await QuanLyNetContext.PhuThuongs.ToListAsync();
                        return View(phanThuong);
                    }

                    var phuThuongExists = await QuanLyNetContext.PhuThuongs.AnyAsync(pt => pt.MaPhuThuong == phanThuong.MaPhuThuong);
                    if (!phuThuongExists)
                    {
                        ModelState.AddModelError("MaPhuThuong", "Mã phần thưởng không tồn tại.");
                        ViewBag.KhachHangs = await QuanLyNetContext.KhachHangs.ToListAsync();
                        ViewBag.PhuThuongs = await QuanLyNetContext.PhuThuongs.ToListAsync();
                        return View(phanThuong);
                    }

                    QuanLyNetContext.Update(phanThuong);
                    await QuanLyNetContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhanThuongDaNhanExists(phanThuong.MaPhanThuong))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.KhachHangs = await QuanLyNetContext.KhachHangs.ToListAsync();
            ViewBag.PhuThuongs = await QuanLyNetContext.PhuThuongs.ToListAsync();
            return View(phanThuong);
        }

        // GET: PhanThuongDaNhan/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var phanThuong = await QuanLyNetContext.PhanThuongDaNhans
                .Include(p => p.MaKhachHangNavigation)
                .Include(p => p.MaPhuThuongNavigation)
                .FirstOrDefaultAsync(p => p.MaPhanThuong == id);
            if (phanThuong == null)
            {
                return NotFound();
            }
            return View(phanThuong);
        }

        // POST: PhanThuongDaNhan/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phanThuong = await QuanLyNetContext.PhanThuongDaNhans.FindAsync(id);
            if (phanThuong != null)
            {
                QuanLyNetContext.PhanThuongDaNhans.Remove(phanThuong);
                await QuanLyNetContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PhanThuongDaNhanExists(int id)
        {
            return QuanLyNetContext.PhanThuongDaNhans.Any(e => e.MaPhanThuong == id);
        }
    }
}