using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using QuanlyquanNet.ViewModel;
using System;
using System.Collections.Generic;
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

        private Dictionary<string, List<(string TenNhiemVu, string MoTa, int DiemThuong)>> GetGameMissions()
        {
            return new Dictionary<string, List<(string, string, int)>>()
            {
                ["Valorant"] = new List<(string, string, int)>
                {
                    ("One Tap Master", "Thực hiện 5 headshot liên tiếp trong 1 trận", 100),
                    ("Smoke Ninja", "Sử dụng smoke để hỗ trợ tiêu diệt 3 kẻ địch", 80),
                    ("Bomb Hero", "Gỡ bom thành công 2 lần", 90)
                },
                ["League of Legends"] = new List<(string, string, int)>
                {
                    ("Trùm Penta", "Thực hiện 1 pha pentakill", 150),
                    ("Thủ Trụ", "Phá 3 trụ trong 1 trận đấu", 70),
                    ("Đi Rừng Siêu Cấp", "Hạ gục 2 rồng nguyên tố", 90)
                },
                ["CS:GO"] = new List<(string, string, int)>
                {
                    ("Bomb Defuser", "Gỡ bom thành công mà không bị hạ gục", 100),
                    ("Ace Hunter", "Hạ gục toàn bộ team địch 1 mình", 120),
                    ("Sniper King", "Tiêu diệt 5 địch bằng AWP", 90)
                },
                ["Dota 2"] = new List<(string, string, int)>
                {
                    ("Roshan Slayer", "Giết Roshan mà không mất đồng đội", 110),
                    ("Ganker Pro", "Tham gia 10 mạng hỗ trợ", 70),
                    ("Godlike", "Đạt chuỗi godlike không chết", 130)
                },
                ["PUBG"] = new List<(string, string, int)>
                {
                    ("Top 1", "Chiến thắng trận đấu", 150),
                    ("Kẻ Hủy Diệt", "Tiêu diệt ít nhất 8 kẻ địch", 100),
                    ("Sát Thủ Lén Lút", "Hạ 3 địch mà không bị phát hiện", 90)
                }
            };
        }

        public IActionResult Index(int? khachHangId)
        {
            var gameMissions = GetGameMissions();

            var khachHangs = _context.KhachHangs.ToList();
            ViewBag.Customers = khachHangs;

            int selectedKhachHangId = khachHangId ?? khachHangs.FirstOrDefault()?.MaKhachHang ?? 0;

            // Lấy nhiệm vụ chưa hoàn thành của khách hàng hiện tại
            var unfinishedTasks = _context.KhachHangNhiemVus
                .Where(x => x.MaKhachHang == khachHangId && x.TrangThai != "Hoàn thành")
                .Include(x => x.MaKhachHangNavigation)
                .Include(x => x.MaNhiemVuNavigation)
                .OrderByDescending(x => x.NgayThamGia)
                .ToList();
            // Bước 1: Truy vấn dữ liệu từ DB, KHÔNG dùng toán tử null
            var rawData = _context.KhachHangNhiemVus
                        .Include(x => x.MaKhachHangNavigation)
                        .Include(x => x.MaNhiemVuNavigation)
                        .OrderByDescending(x => x.NgayThamGia)
                        .Take(10)
                        .ToList();

            // Bước 2: Mapping sang ViewModel trong C#
            var assignedTasks = rawData.Where(x => x.TrangThai != "Hoàn thành")
                .Select(x =>
            {
                var khachHang = x.MaKhachHangNavigation?.HoTen;
                var nhiemVu = x.MaNhiemVuNavigation;
                var game = gameMissions.FirstOrDefault(g =>
                    g.Value.Any(m =>
                        m.TenNhiemVu == nhiemVu?.TenNhiemVu &&
                        m.MoTa == nhiemVu?.MoTa &&
                        m.DiemThuong == nhiemVu?.DiemThuong)).Key ?? "Không xác định";

                return new AssignedTaskViewModel
                {
                    Id = x.MaNhiemVu,
                    KhachHang = khachHang,
                    TenNhiemVu = nhiemVu?.TenNhiemVu,
                    MoTa = nhiemVu?.MoTa,
                    DiemThuong = nhiemVu?.DiemThuong ?? 0,
                    NgayThamGia = x.NgayThamGia,
                    TrangThai = x.TrangThai,
                    Game = game
                };
            }).ToList();

            ViewBag.AssignedTasks = assignedTasks;
            ViewBag.Tasks = unfinishedTasks.Select(x => x.MaNhiemVuNavigation).Distinct().ToList(); // chỉ lấy nhiệm vụ liên quan

            ViewBag.SelectedCustomerId = selectedKhachHangId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(int customerId, string gameName)
        {
            var customer = _context.KhachHangs.FirstOrDefault(x => x.MaKhachHang == customerId);
            if (customer == null)
            {
                ModelState.AddModelError("", "Không tìm thấy khách hàng.");
                return RedirectToAction("Index");
            }

            var gameMissions = GetGameMissions();

            if (!gameMissions.ContainsKey(gameName))
            {
                ModelState.AddModelError("", "Tên game không hợp lệ.");
                return RedirectToAction("Index");
            }

            var selectedGameMissions = gameMissions[gameName];
            var random = new Random();
            var chosen = selectedGameMissions[random.Next(selectedGameMissions.Count)];

            var mission = _context.NhiemVus
                .FirstOrDefault(x => x.TenNhiemVu == chosen.TenNhiemVu && x.MoTa == chosen.MoTa);

            if (mission == null)
            {
                mission = new NhiemVu
                {
                    TenNhiemVu = chosen.TenNhiemVu,
                    MoTa = chosen.MoTa,
                    DiemThuong = chosen.DiemThuong
                };
                _context.NhiemVus.Add(mission);
                _context.SaveChanges();
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

            TempData["Success"] = $"Đã giao nhiệm vụ “{mission.TenNhiemVu}” cho {customer.HoTen}.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetUncompletedTasks(int customerId)
        {
            var gameMissions = GetGameMissions();

            var unfinishedTasks = _context.KhachHangNhiemVus
                .Where(x => x.MaKhachHang == customerId && x.TrangThai != "Hoàn thành")
                .Include(x => x.MaNhiemVuNavigation)
                .ToList();

            var tasks = unfinishedTasks.Select(x =>
            {
                var nhiemVu = x.MaNhiemVuNavigation;

                // Tìm tên game tương ứng
                var game = gameMissions.FirstOrDefault(g =>
                    g.Value.Any(m =>
                        m.TenNhiemVu == nhiemVu.TenNhiemVu &&
                        m.MoTa == nhiemVu.MoTa &&
                        m.DiemThuong == nhiemVu.DiemThuong)).Key ?? "Không xác định";

                return new
                {
                    id = x.MaNhiemVu,
                    tenNhiemVu = nhiemVu.TenNhiemVu,
                    moTa = nhiemVu.MoTa,
                    diemThuong = nhiemVu.DiemThuong,
                    game = game
                };
            });

            return Json(tasks);
        }

        [HttpPost]
        public IActionResult Complete(int taskId)
        {
            var task = _context.KhachHangNhiemVus
                .Include(x => x.MaKhachHangNavigation)
                .FirstOrDefault(x => x.MaNhiemVu == taskId);

            if (task != null)
            {
                task.TrangThai = "Hoàn thành"; // ✅ sửa cho khớp với constraint
                task.ThoiGianHoanThanh = DateTime.Now;
                _context.SaveChanges();
                TempData["Success"] = $"Đã đánh dấu hoàn thành nhiệm vụ cho {task.MaKhachHangNavigation?.HoTen}.";
            }

            return RedirectToAction("Index");
        }
    }
}
