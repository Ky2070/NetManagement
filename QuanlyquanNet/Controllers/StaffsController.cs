using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using QuanlyquanNet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    public class StaffsController : Controller
    {
        private readonly QuanLyNetContext _context;

        public StaffsController(QuanLyNetContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var nhanViens = _context.NhanViens
                .Include(n => n.MaNguoiDungNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                nhanViens = nhanViens.Where(n =>
                    n.HoTen.Contains(searchString) ||
                    n.SoDienThoai.Contains(searchString));
            }

            return View(nhanViens.ToList());
        }

        // Form thêm nhân viên
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddNhanVienViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra trùng tên đăng nhập
            var existing = await _context.NguoiDungs
                .FirstOrDefaultAsync(nd => nd.TenDangNhap == model.TenDangNhap);
            if (existing != null)
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
                return View(model);
            }

            // Tạo người dùng mới (Mật khẩu dạng plain text: "123456")
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = model.TenDangNhap,
                MatKhau = "123456", // ⚠️ Plain text, KHÔNG AN TOÀN nếu dùng thật
                MaVaiTro = 2, // mã vai trò cho nhân viên
                NgayTao = DateTime.Now
            };

            _context.NguoiDungs.Add(nguoiDung);
            await _context.SaveChangesAsync();

            // Tạo nhân viên liên kết với người dùng vừa tạo
            var nhanVien = new NhanVien
            {
                MaNguoiDung = nguoiDung.MaNguoiDung,
                HoTen = model.HoTen,
                SoDienThoai = model.SoDienThoai,
                NgayTao = DateTime.Now
            };

            _context.NhanViens.Add(nhanVien);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        // Xem chi tiết nhân viên
        public async Task<IActionResult> Detail(int id)
        {
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(nv => nv.MaNhanVien == id);
            if (nhanVien == null) return NotFound();
            return View(nhanVien);
        }

        // Form cập nhật nhân viên
        public async Task<IActionResult> Update(int id)
        {
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(nv => nv.MaNhanVien == id);
            if (nhanVien == null) return NotFound();

            ViewBag.NguoiDungs = _context.NguoiDungs.ToList();
            return View(nhanVien);
        }

        // Xử lý cập nhật nhân viên (POST)
        [HttpPost]
        public async Task<IActionResult> Update(NhanVien nhanVien)
        {
            if (!ModelState.IsValid)
                return View(nhanVien);

            var existingNhanVien = await _context.NhanViens.FindAsync(nhanVien.MaNhanVien);
            if (existingNhanVien == null)
                return NotFound();

            existingNhanVien.HoTen = nhanVien.HoTen;
            existingNhanVien.SoDienThoai = nhanVien.SoDienThoai;
            existingNhanVien.NgayTao = nhanVien.NgayTao;
            // Không cập nhật MaNguoiDung (nếu bạn không cho sửa)

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        // Xác nhận xóa nhân viên
        public async Task<IActionResult> Delete(int id)
        {
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(nv => nv.MaNhanVien == id);
            if (nhanVien == null) return NotFound();
            return View(nhanVien);
        }

        // Xử lý xóa nhân viên (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}