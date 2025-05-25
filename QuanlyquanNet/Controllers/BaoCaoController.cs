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
            // Giả sử bạn có bảng nhân viên với username làm khóa
            var nhanVien = _context.NhanViens.FirstOrDefault(nv => nv.MaNguoiDungNavigation.TenDangNhap == username);
            var hoTen = nhanVien?.HoTen ?? username;
            var ngayTao = DateTime.Now;
            var fileName = $"baocao_{ngayTao:yyyyMMdd_HHmmss}.pdf";
            // ✅ Đảm bảo thư mục tồn tại
            var folderPath = Path.Combine(_env.WebRootPath, "Reports", "Choduyet");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, fileName);
            // Tạo file PDF bằng PdfSharpCore
            var document = new PdfSharpCore.Pdf.PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var fontTitle = new XFont("Arial", 20, XFontStyle.Bold);
            var fontNormal = new XFont("Arial", 12);

            double y = 40;
            double lineHeight = 25;

            gfx.DrawString("BÁO CÁO THỐNG KÊ DOANH THU", fontTitle, XBrushes.Black, new XPoint(40, y));
            y += lineHeight * 2;

            gfx.DrawString($"Ngày thống kê: {thongKeGanNhat.NgayThongKe:dd/MM/yyyy}", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"Tổng doanh thu: {thongKeGanNhat.TongDoanhThu:N0} VND", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"- Doanh thu phiên: {thongKeGanNhat.TongDoanhThuPhien:N0} VND", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"- Doanh thu đơn hàng: {thongKeGanNhat.TongDoanhThuDonHang:N0} VND", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"Số máy hoạt động: {thongKeGanNhat.SoMayHoatDong}", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"Tổng số phiên: {thongKeGanNhat.TongSoPhien}", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"Tổng số đơn hàng: {thongKeGanNhat.TongSoDonHang}", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"Số khách hàng: {thongKeGanNhat.SoKhachHang}", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"Ngày tạo thống kê: {thongKeGanNhat.NgayTao?.ToString("dd/MM/yyyy HH:mm")}", fontNormal, XBrushes.Black, new XPoint(40, y)); y += lineHeight;
            gfx.DrawString($"Báo cáo tạo bởi nhân viên: {hoTen}", fontNormal, XBrushes.Black, new XPoint(40, y));

            document.Save(filePath);
            document.Close();

            // Lưu metadata vào file JSON
            var metaFilePath = Path.Combine(_env.WebRootPath, "Reports", "metadata.json");

            var metadataList = new List<ReportMetadata>();

            if (System.IO.File.Exists(metaFilePath))
            {
                var json = System.IO.File.ReadAllText(metaFilePath);
                metadataList = JsonSerializer.Deserialize<List<ReportMetadata>>(json) ?? new List<ReportMetadata>();
            }

            metadataList.Add(new ReportMetadata
            {
                FileName = fileName,
                NguoiTao = hoTen,
                NgayTao = ngayTao,
                TrangThai = "ChoDuyet",
                LyDoTuChoi = null
            });

            System.IO.File.WriteAllText(metaFilePath, JsonSerializer.Serialize(metadataList));

            TempData["msg"] = "✅ Tạo báo cáo thành công. Đang chờ duyệt.";
            return RedirectToAction("TaoBaoCao");
        }
    }
}
