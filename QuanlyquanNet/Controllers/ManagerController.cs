using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanlyquanNet.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
