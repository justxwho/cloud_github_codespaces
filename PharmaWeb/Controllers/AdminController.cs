using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaWeb.Models;
using PharmaWeb.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace PharmaWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ======================
        // CHECK ADMIN
        // ======================
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        // ======================
        // DASHBOARD
        // ======================
        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            ViewBag.TongThuoc = _context.Thuoc.Count();
            ViewBag.TongLoaiThuoc = _context.LoaiThuoc.Count();
            ViewBag.TongKhachHang = _context.KhachHang.Count();
            ViewBag.TongDonHang = _context.DonHang.Count();

            ViewBag.ChoXacNhan = _context.DonHang
                .Count(d => d.TrangThai == "Chờ xác nhận");

            var donHoanTat = _context.DonHang
                .Where(d => d.TrangThai == "Hoàn tất")
                .ToList();

            ViewBag.TongDoanhThu = donHoanTat.Sum(d => d.TongTien ?? 0);

            ViewBag.DonHangMoi = _context.DonHang
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaDsNavigation)
                .OrderByDescending(d => d.MaDh)
                .Take(5)
                .ToList();

            return View();
        }

        // ======================
        // UPLOAD ẢNH
        // ======================
        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            if (!IsAdmin()) return Unauthorized();

            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "Chưa chọn file" });

            var uploadPath = Path.Combine(_env.WebRootPath, "img");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return Json(new { success = true, fileName });
        }

        // ======================
        // QUẢN LÝ THUỐC
        // ======================
        public IActionResult Thuoc()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var list = _context.Thuoc
                .Include(t => t.MaLoaiNavigation)
                .OrderByDescending(t => t.MaThuoc)
                .ToList();

            return View(list);
        }

        // ======================
        // CREATE THUỐC
        // ======================
        [HttpPost]
        public IActionResult CreateThuoc(Thuoc model)
        {
            if (!IsAdmin()) return Json(new { success = false });

            try
            {
                _context.Thuoc.Add(model);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ======================
        // EDIT THUỐC
        // ======================
        [HttpPost]
        public IActionResult EditThuoc(Thuoc model)
        {
            if (!IsAdmin()) return Json(new { success = false });

            try
            {
                var thuoc = _context.Thuoc.FirstOrDefault(t => t.MaThuoc == model.MaThuoc);
                if (thuoc == null)
                    return Json(new { success = false, message = "Không tìm thấy thuốc" });

                thuoc.TenThuoc = model.TenThuoc;
                thuoc.Gia = model.Gia;
                thuoc.SoLuong = model.SoLuong;
                thuoc.MaLoai = model.MaLoai;
                thuoc.CanKeDon = model.CanKeDon;
                thuoc.MoTa = model.MoTa;
                thuoc.Anh = model.Anh;

                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ======================
        // DELETE THUỐC
        // ======================
        [HttpPost]
        public IActionResult DeleteThuoc(int id)
        {
            if (!IsAdmin()) return Json(new { success = false });

            try
            {
                var thuoc = _context.Thuoc.FirstOrDefault(t => t.MaThuoc == id);
                if (thuoc == null)
                    return Json(new { success = false, message = "Không tìm thấy thuốc" });

                // ❗ chặn xóa nếu thuốc đã có trong đơn hàng
                bool daBan = _context.ChiTietDonHang.Any(ct => ct.MaThuoc == id);
                if (daBan)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không thể xóa thuốc đã có trong đơn hàng"
                    });
                }

                _context.Thuoc.Remove(thuoc);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // ======================
        // ĐƠN HÀNG
        // ======================
        public IActionResult DonHang()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var list = _context.DonHang
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaDsNavigation)
                .OrderByDescending(d => d.MaDh)
                .ToList();

            return View(list);
        }

        // Chi tiết đơn hàng
        public IActionResult ChiTietDonHang(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var donHang = _context.DonHang
                .Include(d => d.MaKhNavigation)
                .Include(d => d.MaDsNavigation)
                .FirstOrDefault(d => d.MaDh == id);

            if (donHang == null) return NotFound();

            var chiTiet = _context.ChiTietDonHang
                .Include(c => c.MaThuocNavigation)
                .Where(c => c.MaDh == id)
                .ToList();

            ViewBag.DonHang = donHang;
            return View(chiTiet);
        }

        // Cập nhật trạng thái
        [HttpPost]
        public IActionResult CapNhatTrangThai(int id, string trangThai)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var dh = _context.DonHang.Find(id);
            if (dh == null) return Json(new { success = false });

            dh.TrangThai = trangThai;
            _context.SaveChanges();

            return Json(new { success = true });
        }

        // ======================
        // KHÁCH HÀNG
        // ======================
        public IActionResult KhachHang()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var list = _context.KhachHang
                .Include(k => k.MaTkNavigation)
                .ToList();

            return View(list);
        }

        // ======================
        // KHÓA / MỞ KHÓA TÀI KHOẢN KHÁCH HÀNG
        // ======================
        [HttpPost]
        public IActionResult ToggleKhoaTaiKhoan(int maTk)
        {
            if (!IsAdmin()) return Json(new { success = false });

            var tk = _context.TaiKhoan.FirstOrDefault(t => t.MaTk == maTk);
            if (tk == null)
                return Json(new { success = false, message = "Không tìm thấy tài khoản" });

            // Đảo trạng thái
            tk.TrangThai = tk.TrangThai == 1 ? 0 : 1;
            _context.SaveChanges();

            return Json(new
            {
                success = true,
                trangThai = tk.TrangThai
            });
        }


        // ======================
        // DƯỢC SĨ
        // ======================
        public IActionResult DuocSi()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var list = _context.DuocSi
                .Include(d => d.MaTkNavigation)
                .ToList();

            return View(list);
        }
    }
}
