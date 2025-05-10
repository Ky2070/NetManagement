using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    public class PhuThuongController : Controller
    {
        private readonly QuanLyNetContext _context;

        public PhuThuongController(QuanLyNetContext context)
        {
            _context = context;
        }

        // Trang danh sách phần thưởng
        public async Task<IActionResult> Index()
        {
            var phuThuongs = await _context.PhuThuongs.ToListAsync();
            return View(phuThuongs);
        }

        // Form thêm phần thưởng
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhuThuong model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.NgayTao = DateTime.Now;
            _context.PhuThuongs.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Xem chi tiết phần thưởng
        public async Task<IActionResult> Details(int id)
        {
            var item = await _context.PhuThuongs
                .FirstOrDefaultAsync(p => p.MaPhuThuong == id);
            if (item == null) return NotFound();

            return View(item);
        }

        // Form cập nhật phần thưởng
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.PhuThuongs.FindAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PhuThuong model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _context.PhuThuongs.FindAsync(model.MaPhuThuong);
            if (existing == null) return NotFound();

            existing.TenPhuThuong = model.TenPhuThuong;
            existing.DiemCanDoi = model.DiemCanDoi;
            existing.NgayTao = model.NgayTao;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Xác nhận xoá phần thưởng
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.PhuThuongs.FindAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        // Xử lý xoá phần thưởng
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.PhuThuongs.FindAsync(id);
            if (item != null)
            {
                _context.PhuThuongs.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
