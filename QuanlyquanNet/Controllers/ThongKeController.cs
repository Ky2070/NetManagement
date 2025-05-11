using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QuanlyquanNet.Controllers
{
    [Route("[controller]")]
    public class ThongKeController(QuanLyNetContext context) : Controller
    {
        private readonly QuanLyNetContext _context = context;

        // GET: ThongKe
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var thongKes = await _context.ThongKes.ToListAsync();
            return View(thongKes);
        }

        // GET: ThongKe/Dashboard
        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            // Lấy tất cả thống kê
            var thongKes = await _context.ThongKes.ToListAsync();

            if (thongKes.Count == 0)
            {
                return View(new DashboardViewModel());
            }

            // Gom nhóm dữ liệu theo tháng để so sánh
            var monthlyData = thongKes
                .GroupBy(tk => new { tk.NgayThongKe.Year, tk.NgayThongKe.Month })
                .Select(g => new
                {
                    YearMonth = $"{g.Key.Month}/{g.Key.Year}",
                    TongDoanhThu = g.Sum(tk => tk.TongDoanhThu ?? 0),
                    TongDoanhThuPhien = g.Sum(tk => tk.TongDoanhThuPhien ?? 0),
                    TongDoanhThuDonHang = g.Sum(tk => tk.TongDoanhThuDonHang ?? 0)
                })
                .OrderBy(m => m.YearMonth)
                .ToList();

            // Tính tổng doanh thu
            var totalRevenue = thongKes.Sum(tk => tk.TongDoanhThu ?? 0);
            var totalRevenuePhien = thongKes.Sum(tk => tk.TongDoanhThuPhien ?? 0);
            var totalRevenueDonHang = thongKes.Sum(tk => tk.TongDoanhThuDonHang ?? 0);

            // Tính tăng trưởng (so sánh 2 tháng gần nhất nếu có)
            decimal revenueGrowth = 0;
            if (monthlyData.Count >= 2)
            {
                var lastMonth = monthlyData[monthlyData.Count - 1].TongDoanhThu;
                var previousMonth = monthlyData[monthlyData.Count - 2].TongDoanhThu;
                if (previousMonth != 0)
                {
                    revenueGrowth = ((lastMonth - previousMonth) / previousMonth) * 100;
                }
            }

            // Tính ROS (Return on Sales) - giả định là tỷ lệ doanh thu đơn hàng trên tổng doanh thu
            decimal ros = totalRevenue != 0 ? (totalRevenueDonHang / totalRevenue) * 100 : 0;

            // Chuẩn bị dữ liệu cho biểu đồ
            var viewModel = new DashboardViewModel
            {
                TotalRevenue = totalRevenue,
                TotalRevenuePhien = totalRevenuePhien,
                TotalRevenueDonHang = totalRevenueDonHang,
                RevenueGrowth = revenueGrowth,
                ROS = ros,
                MonthlyRevenueLabels = monthlyData.Select(m => m.YearMonth).ToList(),
                MonthlyRevenueData = monthlyData.Select(m => m.TongDoanhThu).ToList(),
                MonthlyRevenuePhienData = monthlyData.Select(m => m.TongDoanhThuPhien).ToList(),
                MonthlyRevenueDonHangData = monthlyData.Select(m => m.TongDoanhThuDonHang).ToList()
            };

            return View(viewModel);
        }
    }

    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenuePhien { get; set; }
        public decimal TotalRevenueDonHang { get; set; }
        public decimal RevenueGrowth { get; set; }
        public decimal ROS { get; set; }
        public List<string> MonthlyRevenueLabels { get; set; } = new List<string>();
        public List<decimal> MonthlyRevenueData { get; set; } = new List<decimal>();
        public List<decimal> MonthlyRevenuePhienData { get; set; } = new List<decimal>();
        public List<decimal> MonthlyRevenueDonHangData { get; set; } = new List<decimal>();
    }
}