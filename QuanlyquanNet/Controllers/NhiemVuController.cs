using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    public class NhiemVuController : Controller
    {
        private readonly QuanLyNetContext _context;

        public NhiemVuController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: Danh sách nhiệm vụ
        public async Task<IActionResult> Index()
        {
            var nhiemVus = await _context.NhiemVus
                .Include(nv => nv.MaPhuThuongNavigation)
                .ToListAsync();
            return View(nhiemVus);
        }

        // GET: Form thêm nhiệm vụ
        public IActionResult Add()
        {
            ViewBag.MaPhuThuong = _context.PhuThuongs
                .Select(p => new SelectListItem
                {
                    Value = p.MaPhuThuong.ToString(),
                    Text = p.TenPhuThuong
                }).ToList();

            return View();
        }

        // POST: Xử lý thêm nhiệm vụ
        [HttpPost]
        public async Task<IActionResult> Add(NhiemVu nhiemVu)
        {
            if (ModelState.IsValid)
            {
                nhiemVu.NgayTao = DateTime.Now;
                _context.NhiemVus.Add(nhiemVu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Gán lại dropdown khi ModelState invalid
            ViewBag.MaPhuThuong = _context.PhuThuongs
                .Select(p => new SelectListItem
                {
                    Value = p.MaPhuThuong.ToString(),
                    Text = p.TenPhuThuong
                }).ToList();

            return View(nhiemVu);
        }


        // GET: Xem chi tiết nhiệm vụ
        public async Task<IActionResult> Detail(int id)
        {
            var nhiemVu = await _context.NhiemVus
                .Include(nv => nv.MaPhuThuongNavigation)
                .FirstOrDefaultAsync(nv => nv.MaNhiemVu == id);
            if (nhiemVu == null) return NotFound();
            return View(nhiemVu);
        }

        // GET: Form cập nhật nhiệm vụ
        public async Task<IActionResult> Update(int id)
        {
            var nhiemVu = await _context.NhiemVus
                .Include(nv => nv.MaPhuThuongNavigation)
                .FirstOrDefaultAsync(nv => nv.MaNhiemVu == id);
            if (nhiemVu == null) return NotFound();

            ViewBag.PhuThuongs = _context.PhuThuongs.ToList();
            return View(nhiemVu);
        }

        // POST: Xử lý cập nhật nhiệm vụ
        [HttpPost]
        public async Task<IActionResult> Update(NhiemVu nhiemVu)
        {
            if (ModelState.IsValid)
            {
                var existingNhiemVu = await _context.NhiemVus.FindAsync(nhiemVu.MaNhiemVu);
                if (existingNhiemVu == null) return NotFound();

                existingNhiemVu.TenNhiemVu = nhiemVu.TenNhiemVu;
                existingNhiemVu.MoTa = nhiemVu.MoTa;
                existingNhiemVu.MaPhuThuong = nhiemVu.MaPhuThuong;
                existingNhiemVu.DoKho = nhiemVu.DoKho;
                existingNhiemVu.NgayTao = nhiemVu.NgayTao;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PhuThuongs = _context.PhuThuongs.ToList();
            return View(nhiemVu);
        }


        // GET: Xác nhận xóa nhiệm vụ
        public async Task<IActionResult> Delete(int id)
        {
            var nhiemVu = await _context.NhiemVus
                .Include(nv => nv.MaPhuThuongNavigation)
                .FirstOrDefaultAsync(nv => nv.MaNhiemVu == id);
            if (nhiemVu == null) return NotFound();
            return View(nhiemVu);
        }

        // POST: Xử lý xóa nhiệm vụ
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhiemVu = await _context.NhiemVus.FindAsync(id);
            if (nhiemVu != null)
            {
                _context.NhiemVus.Remove(nhiemVu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}