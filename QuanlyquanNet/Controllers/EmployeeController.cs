using Microsoft.AspNetCore.Mvc;

namespace QuanlyquanNet.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
