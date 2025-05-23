﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanlyquanNet.Controllers
{
    [Route("[controller]")]
    public class GoiNapController : Controller
    {
        private readonly QuanLyNetContext _context;

        public GoiNapController(QuanLyNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var goiNaps = await _context.GoiNaps
                .Include(g => g.MaKhuyenMaiNavigation)
                .ToListAsync();
            return View(goiNaps);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var goiNap = await _context.GoiNaps
                .Include(g => g.MaKhuyenMaiNavigation)
                .FirstOrDefaultAsync(g => g.MaGoiNap == id);
            if (goiNap == null)
            {
                return NotFound();
            }
            return View(goiNap);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenGoi,SoTien,MaKhuyenMai")] GoiNap goiNap)
        {
            ModelState.Remove("MaKhuyenMaiNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    goiNap.NgayTao = DateTime.Now;
                    _context.Add(goiNap);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.InnerException?.Message ?? ex.Message);
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                ModelState.AddModelError("", "Dữ liệu không hợp lệ: " + string.Join("; ", errors));
            }
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", goiNap.MaKhuyenMai);
            return View(goiNap);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var goiNap = await _context.GoiNaps.FindAsync(id);
            if (goiNap == null)
            {
                return NotFound();
            }
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", goiNap.MaKhuyenMai);
            return View(goiNap);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGoiNap,TenGoi,SoTien,MaKhuyenMai,NgayTao")] GoiNap goiNap)
        {
            if (id != goiNap.MaGoiNap)
            {
                return NotFound();
            }

            ModelState.Remove("MaKhuyenMaiNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goiNap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoiNapExists(goiNap.MaGoiNap))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", goiNap.MaKhuyenMai);
            return View(goiNap);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var goiNap = await _context.GoiNaps
                .Include(g => g.MaKhuyenMaiNavigation)
                .FirstOrDefaultAsync(g => g.MaGoiNap == id);
            if (goiNap == null)
            {
                return NotFound();
            }
            return View(goiNap);
        }

        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goiNap = await _context.GoiNaps.FindAsync(id);
            if (goiNap != null)
            {
                _context.GoiNaps.Remove(goiNap);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GoiNapExists(int id)
        {
            return _context.GoiNaps.Any(e => e.MaGoiNap == id);
        }
    }
}