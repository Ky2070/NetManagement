using Microsoft.AspNetCore.Mvc;
using QuanlyquanNet.Data;

namespace QuanlyquanNet.Controllers
{
    public class UserController : Controller
    {
        private readonly QuanLyNetContext _context;

        public UserController(QuanLyNetContext context)
        {
            _context = context;
        }
        public IActionResult ServiceDetail()
        {
            return View();
        }

       

        public IActionResult PortfolioDetail()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.NguoiDungs.FirstOrDefault(u =>
            u.TenDangNhap == username && u.MatKhau == password);

            if (user == null) 
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }

            // Ghi lịch sử đăng nhập
            var log = new LichSuDangNhap
            {
                TenDangNhap = user.TenDangNhap,
                HoTen = user.HoTen,
                MaNguoiDung = user.MaNguoiDung,
                ThoiGianDangNhap = DateTime.Now
            };
            _context.LichSuDangNhaps.Add(log);
            _context.SaveChanges();

            // Lưu session
            HttpContext.Session.SetInt32("MaNguoiDung", user.MaNguoiDung);
            HttpContext.Session.SetString("HoTen", user.HoTen);
            HttpContext.Session.SetInt32("MaVaiTro", user.MaVaiTro);

            // Phân quyền redirect
            switch (user.MaVaiTro)
            {
                case 1: // Admin
                    return RedirectToAction("Dashboard", "Admin");

                case 2: // Nhân viên
                    return RedirectToAction("Index", "Manager");

                case 3: // Khách hàng
                    return RedirectToAction("Index", "Home");

                default:
                    return RedirectToAction("Login");
            }

        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session
            return RedirectToAction("Index", "Home");
        }
    }
}
