using Microsoft.AspNetCore.Mvc;
using QuanlyquanNet.Data;
using System;
using System.Linq;
namespace QuanlyquanNet.Controllers
{
    public class TaskController : Controller
    {
        private readonly QuanLyNetContext _context;

        public TaskController(QuanLyNetContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var assignedTasks = _context.KhachHangNhiemVus
                .OrderByDescending(x => x.NgayThamGia)
                .Take(10)
                .Select(x => new
                {
                    KhachHang = x.MaKhachHangNavigation.HoTen,
                    TenNhiemVu = x.MaNhiemVuNavigation.TenNhiemVu,
                    Game = x.MaNhiemVuNavigation.MoTa, // Nếu MoTa dùng để ghi tên game
                    DiemThuong = x.MaNhiemVuNavigation.DiemThuong,
                    NgayThamGia = x.NgayThamGia,
                    TrangThai = x.TrangThai
                })
                .ToList();

            ViewBag.AssignedTasks = assignedTasks;
            ViewBag.Customers = _context.KhachHangs.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(string customerName, string gameName)
        {
            var customer = _context.KhachHangs.FirstOrDefault(x => x.HoTen == customerName);
            if (customer == null)
            {
                ModelState.AddModelError("", "Không tìm thấy khách hàng.");
                return RedirectToAction("Index");
            }

            // Random 1 nhiệm vụ theo game (ví dụ tạm: lọc mô tả chứa tên game)
            var mission = _context.NhiemVus
                .Where(x => x.MoTa != null && x.MoTa.Contains(gameName))
                .OrderBy(r => Guid.NewGuid())
                .FirstOrDefault();

            if (mission == null)
            {
                ModelState.AddModelError("", "Không tìm thấy nhiệm vụ phù hợp.");
                return RedirectToAction("Index");
            }

            var khachHangNhiemVu = new KhachHangNhiemVu
            {
                MaKhachHang = customer.MaKhachHang,
                MaNhiemVu = mission.MaNhiemVu,
                TrangThai = "Chưa hoàn thành",
                DiemNhanDuoc = mission.DiemThuong,
                NgayThamGia = DateTime.Now
            };

            _context.KhachHangNhiemVus.Add(khachHangNhiemVu);
            _context.SaveChanges();

            ViewBag.Customer = customer.HoTen;
            ViewBag.Game = gameName;
            ViewBag.Mission = mission.TenNhiemVu;
            ViewBag.Points = mission.DiemThuong;

            // Cập nhật danh sách nhiệm vụ đã giao
            var assignedTasks = _context.KhachHangNhiemVus
                .OrderByDescending(x => x.NgayThamGia)
                .Take(10)
                .Select(x => new
                {
                    KhachHang = x.MaKhachHangNavigation.HoTen,
                    TenNhiemVu = x.MaNhiemVuNavigation.TenNhiemVu,
                    Game = x.MaNhiemVuNavigation.MoTa,
                    DiemThuong = x.MaNhiemVuNavigation.DiemThuong,
                    NgayThamGia = x.NgayThamGia,
                    TrangThai = x.TrangThai
                })
                .ToList();

            ViewBag.AssignedTasks = assignedTasks;
            ViewBag.Customers = _context.KhachHangs.ToList();
            return View("Index");
        }

        [HttpPost]
        public IActionResult Complete(string customerName)
        {
            var customer = _context.KhachHangs.FirstOrDefault(x => x.HoTen == customerName);
            if (customer != null)
            {
                var task = _context.KhachHangNhiemVus
                    .Where(x => x.MaKhachHang == customer.MaKhachHang && x.TrangThai == "Chưa hoàn thành")
                    .OrderByDescending(x => x.NgayThamGia)
                    .FirstOrDefault();

                if (task != null)
                {
                    task.TrangThai = "Đã hoàn thành";
                    task.ThoiGianHoanThanh = DateTime.Now;
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
