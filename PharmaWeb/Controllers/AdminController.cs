using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaWeb.Models;
using PharmaWeb.Data;
using Microsoft.AspNetCore.Hosting; 
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace PharmaWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env; // Thay thế Server.MapPath

        public AdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // Middleware kiểm tra Admin thủ công
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            // Thống kê cơ bản
            ViewBag.TongThuoc = _context.Thuoc.Count();
            ViewBag.TongDonHang = _context.DonHang.Count();
            ViewBag.TongKhachHang = _context.KhachHang.Count();
            
            // Tính tổng doanh thu (SQLite cẩn thận với Sum null)
            var listDonHang = _context.DonHang.Where(h => h.TrangThai == "Hoàn tất").ToList();
            ViewBag.TongDoanhThu = listDonHang.Sum(h => h.TongTien ?? 0);

            return View();
        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            if (!IsAdmin()) return Unauthorized();

            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "Chưa chọn file" });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "img");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Json(new { success = true, fileName = fileName });
        }

        public IActionResult DonHang()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var list = _context.DonHang
                .Include(d => d.MaKhNavigation) // Include KhachHang
                .OrderByDescending(d => d.MaDh)
                .ToList();
            return View(list);
        }

        // Cập nhật trạng thái đơn
        [HttpPost]
        public IActionResult CapNhatTrangThai(int id, string trangThai)
        {
             if (!IsAdmin()) return Json(new{success=false});

             var dh = _context.DonHang.Find(id);
             if(dh != null) {
                 dh.TrangThai = trangThai;
                 _context.SaveChanges();
                 return Json(new{success=true});
             }
             return Json(new{success=false});
        }
    }
}