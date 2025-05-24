using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuanlyquanNet.ViewModel;
using System.Text.Json;
namespace QuanlyquanNet.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public AdminController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult DuyetBaoCao()
        {
            var metaFilePath = Path.Combine(_env.WebRootPath, "Reports", "metadata.json");
            var metadataList = new List<ReportMetadata>();

            if (System.IO.File.Exists(metaFilePath))
            {
                var json = System.IO.File.ReadAllText(metaFilePath);
                metadataList = JsonSerializer.Deserialize<List<ReportMetadata>>(json) ?? new List<ReportMetadata>();
            }

            var choDuyet = metadataList.Where(m => m.TrangThai == "ChoDuyet").ToList();
            return View(choDuyet);
        }

        [HttpPost]
        public IActionResult XuLyBaoCao(string fileName, string hanhDong, string? lyDo)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(hanhDong))
                return BadRequest("Thiếu thông tin.");

            hanhDong = hanhDong.Trim();

            if (hanhDong != "Duyet" && hanhDong != "TuChoi")
                return BadRequest("Hành động không hợp lệ.");

            var root = _env.WebRootPath;
            var metaFilePath = Path.Combine(root, "Reports", "metadata.json");
            if (!System.IO.File.Exists(metaFilePath))
                return NotFound("Không tìm thấy dữ liệu metadata.");

            var metadataList = JsonSerializer.Deserialize<List<ReportMetadata>>(System.IO.File.ReadAllText(metaFilePath)) ?? new List<ReportMetadata>();

            var report = metadataList.FirstOrDefault(r => r.FileName == fileName);
            if (report == null)
                return NotFound("Báo cáo không tồn tại.");

            var sourcePath = Path.Combine(root, "Reports", "Choduyet", fileName);
            if (!System.IO.File.Exists(sourcePath))
                return NotFound("File báo cáo gốc không tồn tại.");

            string destDir = hanhDong == "Duyet" ? "DaDuyet" : "TuChoi";
            var destFolder = Path.Combine(root, "Reports", destDir);

            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            var destPath = Path.Combine(destFolder, fileName);

            if (System.IO.File.Exists(destPath))
            {
                // Nếu file đã tồn tại ở thư mục đích, bạn có thể đổi tên hoặc xóa file cũ trước khi di chuyển.
                System.IO.File.Delete(destPath);
            }

            System.IO.File.Move(sourcePath, destPath);

            report.TrangThai = hanhDong == "Duyet" ? "DaDuyet" : "TuChoi";
            report.LyDoTuChoi = hanhDong == "TuChoi" ? lyDo : null;

            System.IO.File.WriteAllText(metaFilePath, JsonSerializer.Serialize(metadataList));

            TempData["msg"] = $"Báo cáo đã được {(hanhDong == "Duyet" ? "duyệt" : "từ chối")} thành công.";

            return RedirectToAction("DuyetBaoCao");
        }
    }
}
