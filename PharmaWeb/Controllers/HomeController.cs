using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaWeb.Models;
using PharmaWeb.Data;
using System;
using System.Linq;

namespace PharmaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? maLoai)
        {
            // Lấy tất cả loại thuốc để hiển thị trong sidebar
            ViewBag.LoaiThuoc = _context.LoaiThuoc.ToList();
            
            // Lọc thuốc theo danh mục nếu có maLoai
            var thuoc = _context.Thuoc.Include(t => t.MaLoaiNavigation).AsQueryable();
            
            if (maLoai.HasValue)
            {
                thuoc = thuoc.Where(t => t.MaLoai == maLoai.Value);
                ViewBag.SelectedCategory = maLoai.Value;
            }
            
            return View(thuoc.ToList());
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 2)
                return Json(new { });

            var products = _context.Thuoc
                .Where(p => p.TenThuoc.Contains(keyword))
                .Take(8)
                .Select(p => new
                {
                    p.MaThuoc,
                    p.TenThuoc,
                    p.Gia,
                    Anh = p.Anh ?? "default.jpg"
                })
                .ToList();

            return Json(products);
        }

        public IActionResult ChiTietSanPham(int id)
        {
            var sp = _context.Thuoc
                .Include(t => t.MaLoaiNavigation) // Include bảng LoaiThuoc
                .FirstOrDefault(p => p.MaThuoc == id);

            if (sp == null) return NotFound();

            var relatedProducts = _context.Thuoc
                .Where(p => p.MaLoai == sp.MaLoai && p.MaThuoc != sp.MaThuoc)
                .Take(4)
                .ToList();

            ViewBag.RelatedProducts = relatedProducts;
            return View(sp);
        }
    }
}