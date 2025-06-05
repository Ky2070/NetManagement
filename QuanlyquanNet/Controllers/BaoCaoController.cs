using System.Text.Json;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanlyquanNet.Data;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.ViewModel;
using iText.Kernel.Pdf;
using System.Drawing;

namespace QuanlyquanNet.Controllers
{
    [Authorize(Roles = "Employee")] // Chỉ cho nhân viên truy cập
    public class BaoCaoController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly QuanLyNetContext _context;

        public BaoCaoController(IWebHostEnvironment env, QuanLyNetContext context)
        {
            _env = env;
            _context = context;
        }

        [HttpGet]
        public IActionResult TaoBaoCao()
        {
            return View(); // View cho nhân viên bấm nút tạo
        }

        [HttpPost]
        public IActionResult TaoBaoCaoSubmit()
        {
            var thongKeGanNhat = _context.ThongKes
                .OrderByDescending(t => t.NgayThongKe)
                .FirstOrDefault();

            if (thongKeGanNhat == null)
            {
                TempData["msg"] = "Không có dữ liệu thống kê để tạo báo cáo.";
                return RedirectToAction("TaoBaoCao");
            }

            var username = User.Identity?.Name ?? "Unknown";
            var nhanVien = _context.NhanViens.FirstOrDefault(nv => nv.MaNguoiDungNavigation.TenDangNhap == username);
            var hoTen = nhanVien?.HoTen ?? username;
            var ngayTao = DateTime.Now;
            var fileName = $"baocao_{ngayTao:yyyyMMdd_HHmmss}.pdf";

            var folderPreview = Path.Combine(_env.WebRootPath, "Reports", "Preview");
            if (!Directory.Exists(folderPreview))
                Directory.CreateDirectory(folderPreview);

            var filePath = Path.Combine(folderPreview, fileName);

            var folderChoDuyet = Path.Combine(_env.WebRootPath, "Reports", "ChoDuyet");
            if (!Directory.Exists(folderChoDuyet))
                Directory.CreateDirectory(folderChoDuyet);

            // Tạo PDF
            var document = new PdfSharpCore.Pdf.PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var fontTitle = new XFont("Arial", 20, XFontStyle.Bold);
            var fontNormal = new XFont("Arial", 12);

            double y = 40;
            double lineHeight = 25;
            var logoPath = Path.Combine(_env.WebRootPath, "assets", "images", "logo.png");
            if (System.IO.File.Exists(logoPath))
            {
                var img = XImage.FromFile(logoPath);
                gfx.DrawImage(img, 40, 20, 100, 100);
            }
            y += 80; // đẩy xuống sau logo

            gfx.DrawString("BÁO CÁO THỐNG KÊ DOANH THU", fontTitle, XBrushes.DarkBlue, new XRect(0, y, page.Width, 40), XStringFormats.TopCenter);
            y += lineHeight * 2;
            gfx.DrawLine(XPens.Black, 40, y, page.Width - 40, y); y += 10;

            gfx.DrawRectangle(XPens.Black, XBrushes.LightGray, 40, y, page.Width - 80, 25);
            gfx.DrawString("Chỉ tiêu", fontNormal, XBrushes.Black, new XRect(50, y + 5, 200, 25), XStringFormats.TopLeft);
            gfx.DrawString("Giá trị", fontNormal, XBrushes.Black, new XRect(300, y + 5, 200, 25), XStringFormats.TopLeft);
            y += 30;

            var data = new (string label, string value)[]
            {
                ("Ngày thống kê", thongKeGanNhat.NgayThongKe.ToString("dd/MM/yyyy")),
                ("Tổng doanh thu", $"{thongKeGanNhat.TongDoanhThu:N0} VND"),
                ("- Doanh thu phiên", $"{thongKeGanNhat.TongDoanhThuPhien:N0} VND"),
                ("- Doanh thu đơn hàng", $"{thongKeGanNhat.TongDoanhThuDonHang:N0} VND"),
                ("Số máy hoạt động", thongKeGanNhat.SoMayHoatDong.ToString()),
                ("Tổng số phiên", thongKeGanNhat.TongSoPhien.ToString()),
                ("Tổng số đơn hàng", thongKeGanNhat.TongSoDonHang.ToString()),
                ("Số khách hàng", thongKeGanNhat.SoKhachHang.ToString()),
            };

            foreach (var (label, value) in data)
            {
                gfx.DrawRectangle(XPens.Black, 40, y, page.Width - 80, 25);
                gfx.DrawString(label, fontNormal, XBrushes.Black, new XRect(50, y + 5, 200, 25), XStringFormats.TopLeft);
                gfx.DrawString(value, fontNormal, XBrushes.Black, new XRect(300, y + 5, 200, 25), XStringFormats.TopLeft);
                y += 25;
            }

            y += 30;
            gfx.DrawString($"Người lập báo cáo: {hoTen}", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"Ngày tạo: {ngayTao:dd/MM/yyyy HH:mm:ss}", fontNormal, XBrushes.Black, new XPoint(40, y));

            document.Save(filePath);
            document.Close();

            var metaFilePath = Path.Combine(_env.WebRootPath, "Reports", "metadata.json");
            var metadataList = new List<ReportMetadata>();

            // Đọc metadata hiện có (nếu có)
            if (System.IO.File.Exists(metaFilePath))
            {
                var existingJson = System.IO.File.ReadAllText(metaFilePath);
                metadataList = JsonSerializer.Deserialize<List<ReportMetadata>>(existingJson) ?? new List<ReportMetadata>();
            }

            // Thêm bản ghi mới
            metadataList.Add(new ReportMetadata
            {
                FileName = fileName,
                NguoiTao = hoTen,
                NgayTao = ngayTao,
                TrangThai = "MoiTao",
                LyDoTuChoi = null
            });

            // Ghi lại toàn bộ danh sách
            var metadataJson = JsonSerializer.Serialize(metadataList, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            System.IO.File.WriteAllText(metaFilePath, metadataJson);

            // Gửi sang trang preview để xem PDF
            return RedirectToAction("XemTruocBaoCao", new { fileName = fileName });
        }

        [HttpGet]
        public IActionResult XemTruocBaoCao(string fileName)
        {
            ViewBag.FileName = fileName;
            return View();
        }

        [HttpPost]
        public IActionResult GuiBaoCaoChoAdmin(string fileName)
        {
            var srcPath = Path.Combine(_env.WebRootPath, "Reports", "Preview", fileName);
            var destPath = Path.Combine(_env.WebRootPath, "Reports", "Choduyet", fileName);

            if (System.IO.File.Exists(srcPath))
            {
                System.IO.File.Move(srcPath, destPath);

                // Cập nhật metadata
                var metaFilePath = Path.Combine(_env.WebRootPath, "Reports", "metadata.json");
                var metadataList = new List<ReportMetadata>();

                if (System.IO.File.Exists(metaFilePath))
                {
                    var json = System.IO.File.ReadAllText(metaFilePath);
                    metadataList = JsonSerializer.Deserialize<List<ReportMetadata>>(json) ?? new List<ReportMetadata>();
                }

                var username = User.Identity?.Name ?? "Unknown";
                var nhanVien = _context.NhanViens.FirstOrDefault(nv => nv.MaNguoiDungNavigation.TenDangNhap == username);
                var hoTen = nhanVien?.HoTen ?? username;

                // ❗️Kiểm tra nếu đã có thì cập nhật, chưa có thì thêm
                var existing = metadataList.FirstOrDefault(x => x.FileName == fileName);
                if (existing != null)
                {
                    existing.TrangThai = "ChoDuyet";
                    existing.NgayTao = DateTime.Now;
                    existing.NguoiTao = hoTen;
                    existing.LyDoTuChoi = null;
                }
                else
                {
                    metadataList.Add(new ReportMetadata
                    {
                        FileName = fileName,
                        NguoiTao = hoTen,
                        NgayTao = DateTime.Now,
                        TrangThai = "ChoDuyet",
                        LyDoTuChoi = null
                    });
                }

                // ❗️Lọc metadataList giữ bản ghi mới nhất cho mỗi FileName
                metadataList = metadataList
                    .GroupBy(x => x.FileName)
                    .Select(g => g.OrderByDescending(x => x.NgayTao).First())
                    .ToList();

                System.IO.File.WriteAllText(metaFilePath, JsonSerializer.Serialize(metadataList, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
            }

            TempData["msg"] = "✅ Gửi báo cáo thành công.";
            return RedirectToAction("TaoBaoCao");
        }
    }
}
