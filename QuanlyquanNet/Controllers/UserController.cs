using Microsoft.AspNetCore.Mvc;
using QuanlyquanNet.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public async Task<IActionResult> Login(string username, string password)
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

            // ✅ Tạo Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.TenDangNhap),
                new Claim(ClaimTypes.Role, GetRoleName(user.MaVaiTro)) // ánh xạ role từ mã
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // ✅ Đăng nhập (ghi cookie)
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

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
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // xóa cookie xác thực
            HttpContext.Session.Clear(); // Xóa toàn bộ session
            return RedirectToAction("Index", "Home");
        }

        // ✅ Hàm ánh xạ mã vai trò thành tên role
        private string GetRoleName(int maVaiTro)
        {
            return maVaiTro switch
            {
                1 => "Admin",
                2 => "Employee",
                3 => "Customer",
                _ => "Unknown"
            };
        }
    }
}
