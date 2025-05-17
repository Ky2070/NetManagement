using Microsoft.AspNetCore.Mvc;

namespace QuanlyquanNet.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
