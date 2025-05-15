using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
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
        public IActionResult Create()
        {
            ViewData["MaDichVu"] = new SelectList(_context.DichVus, "MaDichVu", "TenDichVu");
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen");
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "TenMayTinh");
            return View();
        }

        // POST: DonHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhachHang,MaDichVu,SoLuong,MaMayTinh,ThongTinViTri,TongTien,NgayDat,TrangThai")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaDichVu"] = new SelectList(_context.DichVus, "MaDichVu", "TenDichVu", donHang.MaDichVu);
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", donHang.MaKhachHang);
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "TenMayTinh", donHang.MaMayTinh);
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

    // Kiểm tra dữ liệu trước khi gán ViewData
    var dichVus = await _context.DichVus.ToListAsync();
    var khachHangs = await _context.KhachHangs.ToListAsync();
    var mayTinhs = await _context.MayTinhs.ToListAsync();

    if (!dichVus.Any() || !khachHangs.Any())
    {
        // Nếu không có dữ liệu, trả về thông báo lỗi
        TempData["ErrorMessage"] = "Không có dữ liệu dịch vụ hoặc khách hàng để chỉnh sửa đơn hàng.";
        return RedirectToAction(nameof(Index));
    }

    ViewData["MaDichVu"] = new SelectList(dichVus, "MaDichVu", "TenDichVu", donHang.MaDichVu);
    ViewData["MaMayTinh"] = new SelectList(mayTinhs, "MaMayTinh", "TenMayTinh", donHang.MaMayTinh);
    ViewData["MaKhachHang"] = new SelectList(khachHangs, "MaKhachHang", "HoTen", donHang.MaKhachHang);
        return View(donHang);
}

        // POST: DonHang/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDonHang,MaKhachHang,MaDichVu,SoLuong,MaMayTinh,ThongTinViTri,TongTien,NgayDat,TrangThai")] DonHang donHang)
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.MaDonHang))
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
            ViewData["MaDichVu"] = new SelectList(_context.DichVus, "MaDichVu", "TenDichVu", donHang.MaDichVu);
            ViewData["MaMayTinh"] = new SelectList(_context.MayTinhs, "MaMayTinh", "TenMayTinh", donHang.MaMayTinh);
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "HoTen", donHang.MaKhachHang);


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
            _context.DonHangs.Remove(donHang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.MaDonHang == id);
        }
    }
}
