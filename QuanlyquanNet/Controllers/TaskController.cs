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
            return new Dictionary<string, List<(string, string, int)>>
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

        private Dictionary<(string TenNhiemVu, string MoTa, int DiemThuong), string> GetGameProofImages()
        {
            return new Dictionary<(string, string, int), string>
            {
                [("One Tap Master", "Thực hiện 5 headshot liên tiếp trong 1 trận", 100)] = "https://cmsassets.rgpub.io/sanity/images/dsfx7636/news_live/0fd808037b9c16d9f04cfee0b35b5b3be488f26e-1920x1080.jpg?auto=format&fit=fill&q=80&w=1082",
                [("Smoke Ninja", "Sử dụng smoke để hỗ trợ tiêu diệt 3 kẻ địch", 80)] = "https://gamecuoi.com/wp-content/uploads/2023/05/Nhung-ky-luc-the-gioi-trong-Valorant-1-2a-700x399.jpg",
                [("Bomb Hero", "Gỡ bom thành công 2 lần", 90)] = "https://files.bo3.gg/uploads/image/25499/image/webp-6b1d91b4a2698a89088325e2db19412d.webp",
                [("Trùm Penta", "Thực hiện 1 pha pentakill", 150)] = "https://haycafe.vn/wp-content/uploads/2021/11/Hinh-anh-Pentakill.jpg",
                [("Thủ Trụ", "Phá 3 trụ trong 1 trận đấu", 70)] = "https://memeface.vip/wp-content/uploads/2023/11/loi-che-do-cua-so-lien-minh-huyen-thoai.jpg",
                [("Đi Rừng Siêu Cấp", "Hạ gục 2 rồng nguyên tố", 90)] = "https://cdnmedia.webthethao.vn/uploads/2023-03-21/ky-luc-an-rong.jpg",
                [("Bomb Defuser", "Gỡ bom thành công mà không bị hạ gục", 100)] = "/images/proof/csgo_bomb_proof.jpg",
                [("Ace Hunter", "Hạ gục toàn bộ team địch 1 mình", 120)] = "/images/proof/csgo_ace_proof.jpg",
                [("Sniper King", "Tiêu diệt 5 địch bằng AWP", 90)] = "/images/proof/csgo_sniper_proof.jpg",
                [("Roshan Slayer", "Giết Roshan mà không mất đồng đội", 110)] = "/images/proof/dota_roshan_proof.jpg",
                [("Ganker Pro", "Tham gia 10 mạng hỗ trợ", 70)] = "/images/proof/dota_gank_proof.jpg",
                [("Godlike", "Đạt chuỗi godlike không chết", 130)] = "/images/proof/dota_godlike_proof.jpg",
                [("Top 1", "Chiến thắng trận đấu", 150)] = "/images/proof/pubg_top1_proof.jpg",
                [("Kẻ Hủy Diệt", "Tiêu diệt ít nhất 8 kẻ địch", 100)] = "/images/proof/pubg_destroyer_proof.jpg",
                [("Sát Thủ Lén Lút", "Hạ 3 địch mà không bị phát hiện", 90)] = "/images/proof/pubg_sneak_proof.jpg"
            };
        }

        public IActionResult Index(int? khachHangId)
        {
            var gameMissions = GetGameMissions();

            var khachHangs = _context.KhachHangs.ToList();
            ViewBag.Customers = khachHangs;

            int selectedKhachHangId = khachHangId ?? khachHangs.FirstOrDefault()?.MaKhachHang ?? 0;

            var unfinishedTasks = _context.KhachHangNhiemVus
                .Where(x => x.MaKhachHang == khachHangId && x.TrangThai != "Hoàn thành")
                .Include(x => x.MaKhachHangNavigation)
                .Include(x => x.MaNhiemVuNavigation)
                .OrderByDescending(x => x.NgayThamGia)
                .ToList();

            var rawData = _context.KhachHangNhiemVus
                .Include(x => x.MaKhachHangNavigation)
                .Include(x => x.MaNhiemVuNavigation)
                .OrderByDescending(x => x.NgayThamGia)
                .Take(10)
                .ToList();

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
            ViewBag.Tasks = unfinishedTasks.Select(x => x.MaNhiemVuNavigation).Distinct().ToList();
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

            // Check if the task assignment already exists for this customer
            var existingAssignment = _context.KhachHangNhiemVus
                .FirstOrDefault(x => x.MaKhachHang == customerId && x.MaNhiemVu == mission.MaNhiemVu);

            if (existingAssignment == null)
            {
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
            }
            else
            {
                // Optionally reset the status if the task was previously completed
                if (existingAssignment.TrangThai == "Hoàn thành")
                {
                    existingAssignment.TrangThai = "Chưa hoàn thành";
                    existingAssignment.NgayThamGia = DateTime.Now;
                    _context.SaveChanges();
                    TempData["Success"] = $"Nhiệm vụ “{mission.TenNhiemVu}” đã được giao lại cho {customer.HoTen}.";
                }
                else
                {
                    TempData["Success"] = $"Nhiệm vụ “{mission.TenNhiemVu}” đã tồn tại cho {customer.HoTen}.";
                }
            }

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
                .Include(x => x.MaNhiemVuNavigation)
                .FirstOrDefault(x => x.MaNhiemVu == taskId);

            if (task != null)
            {
                task.TrangThai = "Hoàn thành";
                task.ThoiGianHoanThanh = DateTime.Now;
                _context.SaveChanges();

                var gameMissions = GetGameMissions();
                var proofImages = GetGameProofImages();
                var nhiemVu = task.MaNhiemVuNavigation;
                var taskKey = (nhiemVu.TenNhiemVu, nhiemVu.MoTa, nhiemVu.DiemThuong);
                var proofImage = proofImages.ContainsKey(taskKey) ? proofImages[taskKey] : "/images/proof/default_proof.jpg";

                TempData["Success"] = $"Đã đánh dấu hoàn thành nhiệm vụ cho {task.MaKhachHangNavigation?.HoTen}.";
                TempData["ProofImage"] = proofImage;
            }

            return RedirectToAction("Index");
        }
    }
}