using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    public class DonHangController : Controller
    {
        private readonly QuanLyNetContext _context;

        public DonHangController(QuanLyNetContext context)
        {
            _context = context;
        }

        // GET: DonHang
        public async Task<IActionResult> Index()
        {
            var donHangs = await _context.DonHangs
                .Include(d => d.MaDichVuNavigation)
                .Include(d => d.MaKhachHangNavigation)
                .Include(d => d.MaMayTinhNavigation)
                .ToListAsync();
            return View(donHangs);
        }

        // GET: DonHang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.MaDichVuNavigation)
                .Include(d => d.MaKhachHangNavigation)
                .Include(d => d.MaMayTinhNavigation)
                .FirstOrDefaultAsync(m => m.MaDonHang == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: DonHang/Create
        public async Task<IActionResult> Create()
        {
            var dichVus = await _context.DichVus.ToListAsync();
            var khachHangs = await _context.KhachHangs.ToListAsync();
            var mayTinhs = await _context.MayTinhs.ToListAsync();

            if (!dichVus.Any() || !khachHangs.Any() || !mayTinhs.Any())
            {
                TempData["ErrorMessage"] = "Không có dữ liệu dịch vụ, khách hàng hoặc máy tính để tạo đơn hàng.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaDichVu"] = new SelectList(dichVus, "MaDichVu", "TenDichVu");
            ViewData["MaKhachHang"] = new SelectList(khachHangs, "MaKhachHang", "HoTen");
            ViewData["MaMayTinh"] = new SelectList(mayTinhs, "MaMayTinh", "TenMayTinh");
            return View();
        }

        // POST: DonHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,MaDichVu,MaMayTinh,SoLuong,ThongTinViTri,TongTien,NgayDat,TrangThai")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(donHang);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đơn hàng đã được tạo thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Lỗi khi tạo đơn hàng. Vui lòng thử lại.";
                }
            }

            var dichVus = await _context.DichVus.ToListAsync();
            var khachHangs = await _context.KhachHangs.ToListAsync();
            var mayTinhs = await _context.MayTinhs.ToListAsync();

            ViewData["MaDichVu"] = new SelectList(dichVus, "MaDichVu", "TenDichVu", donHang.MaDichVu);
            ViewData["MaKhachHang"] = new SelectList(khachHangs, "MaKhachHang", "HoTen", donHang.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(mayTinhs, "MaMayTinh", "TenMayTinh", donHang.MaMayTinh);
            return View(donHang);
        }

        // GET: DonHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }

            var dichVus = await _context.DichVus.ToListAsync();
            var khachHangs = await _context.KhachHangs.ToListAsync();
            var mayTinhs = await _context.MayTinhs.ToListAsync();

            if (!dichVus.Any() || !khachHangs.Any() || !mayTinhs.Any())
            {
                TempData["ErrorMessage"] = "Không có dữ liệu dịch vụ, khách hàng hoặc máy tính để chỉnh sửa đơn hàng.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaDichVu"] = new SelectList(dichVus, "MaDichVu", "TenDichVu", donHang.MaDichVu);
            ViewData["MaKhachHang"] = new SelectList(khachHangs, "MaKhachHang", "HoTen", donHang.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(mayTinhs, "MaMayTinh", "TenMayTinh", donHang.MaMayTinh);
            return View(donHang);
        }

        // POST: DonHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDonHang,MaKhachHang,MaDichVu,MaMayTinh,SoLuong,ThongTinViTri,TongTien,NgayDat,TrangThai")] DonHang donHang)
        {
            if (id != donHang.MaDonHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Đơn hàng đã được cập nhật thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.MaDonHang))
                    {
                        return NotFound();
                    }
                    TempData["ErrorMessage"] = "Lỗi đồng bộ hóa dữ liệu. Vui lòng thử lại.";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Lỗi khi cập nhật đơn hàng. Vui lòng thử lại.";
                }
            }

            var dichVus = await _context.DichVus.ToListAsync();
            var khachHangs = await _context.KhachHangs.ToListAsync();
            var mayTinhs = await _context.MayTinhs.ToListAsync();

            ViewData["MaDichVu"] = new SelectList(dichVus, "MaDichVu", "TenDichVu", donHang.MaDichVu);
            ViewData["MaKhachHang"] = new SelectList(khachHangs, "MaKhachHang", "HoTen", donHang.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(mayTinhs, "MaMayTinh", "TenMayTinh", donHang.MaMayTinh);
            return View(donHang);
        }

        // GET: DonHang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.MaDichVuNavigation)
                .Include(d => d.MaKhachHangNavigation)
                .Include(d => d.MaMayTinhNavigation)
                .FirstOrDefaultAsync(m => m.MaDonHang == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // POST: DonHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }

            try
            {
                _context.DonHangs.Remove(donHang);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đơn hàng đã được xóa thành công.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa đơn hàng. Vui lòng thử lại.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDonHang == id);
        }
    }
}