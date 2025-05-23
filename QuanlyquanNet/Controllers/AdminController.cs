using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace QuanlyquanNet.Controllers
{
    [Authorize(Roles = "Admin")] // Chỉ cho phép người dùng có role "Admin"
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
