using Microsoft.AspNetCore.Mvc;

namespace QuanlyquanNet.Controllers
{
    public class UserController : Controller
    {
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
        public IActionResult Register()
        {
            return View();
        }
    }
}
