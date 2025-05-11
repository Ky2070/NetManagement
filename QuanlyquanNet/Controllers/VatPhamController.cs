using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanlyquanNet.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace QuanlyquanNet.Controllers
{
    [Route("[controller]")]
    public class VatPhamController : Controller
    {
        private readonly QuanLyNetContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VatPhamController(QuanLyNetContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: VatPham
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vatPhams = await _context.VatPhams.ToListAsync();
            return View(vatPhams);
        }

        // GET: VatPham/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var vatPham = await _context.VatPhams
                .FirstOrDefaultAsync(vp => vp.MaVatPham == id);
            if (vatPham == null)
            {
                return NotFound();
            }
            return View(vatPham);
        }

        // GET: VatPham/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: VatPham/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenVatPham,MoTa,DiemCanDoi,GiaMua,SoLuongTon")] VatPham vatPham)
        {
            if (ModelState.IsValid)
            {
                vatPham.NgayTao = DateTime.Now;
                _context.Add(vatPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vatPham);
        }

        // GET: VatPham/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var vatPham = await _context.VatPhams.FindAsync(id);
            if (vatPham == null)
            {
                return NotFound();
            }
            return View(vatPham);
        }

        // POST: VatPham/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaVatPham,TenVatPham,MoTa,DiemCanDoi,GiaMua,SoLuongTon,NgayTao")] VatPham vatPham)
        {
            if (id != vatPham.MaVatPham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vatPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VatPhamExists(vatPham.MaVatPham))
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
            return View(vatPham);
        }

        // GET: VatPham/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vatPham = await _context.VatPhams
                .FirstOrDefaultAsync(vp => vp.MaVatPham == id);
            if (vatPham == null)
            {
                return NotFound();
            }
            return View(vatPham);
        }

        // POST: VatPham/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vatPham = await _context.VatPhams.FindAsync(id);
            if (vatPham != null)
            {
                _context.VatPhams.Remove(vatPham);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VatPhamExists(int id)
        {
            return _context.VatPhams.Any(e => e.MaVatPham == id);
        }
    }
}
