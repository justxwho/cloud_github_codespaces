using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaWeb.Models;
using PharmaWeb.Data;
using System;
using System.Linq;

namespace PharmaWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        private GioHang GetOrCreateCart()
        {
            var maTKStr = HttpContext.Session.GetString("MaTK");
            if (string.IsNullOrEmpty(maTKStr)) return null;

            int maTK = int.Parse(maTKStr);
            var cart = _context.GioHang.FirstOrDefault(g => g.MaTk == maTK);

            if (cart == null)
            {
                cart = new GioHang
                {
                    MaTk = maTK,
                    NgayCapNhat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") // SQLite lưu ngày dạng chuỗi
                };
                _context.GioHang.Add(cart);
                _context.SaveChanges();
            }
            return cart;
        }

        public IActionResult Index()
        {
            var cart = GetOrCreateCart();
            if (cart == null) return RedirectToAction("Login", "Account");

            var items = _context.GioHangChiTiet
                            .Include(g => g.MaThuocNavigation) // Include Thuoc để lấy tên/giá
                            .Where(g => g.MaGh == cart.MaGh)
                            .ToList();
            return View(items);
        }

        [HttpPost]
        public IActionResult ThemVaoGio(int maSP, int soLuong = 1)
        {
            var cart = GetOrCreateCart();
            if (cart == null) return Json(new { success = false, message = "Vui lòng đăng nhập!" });

            var item = _context.GioHangChiTiet
                           .FirstOrDefault(g => g.MaGh == cart.MaGh && g.MaThuoc == maSP);

            if (item != null)
            {
                item.SoLuong += soLuong;
            }
            else
            {
                item = new GioHangChiTiet
                {
                    MaGh = cart.MaGh,
                    MaThuoc = maSP,
                    SoLuong = soLuong
                };
                _context.GioHangChiTiet.Add(item);
            }
            
            cart.NgayCapNhat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _context.SaveChanges();

            return Json(new { success = true, message = "Đã thêm thuốc vào giỏ!" });
        }

        [HttpPost]
        public IActionResult ThanhToan(string phuongThuc)
        {
            var cart = GetOrCreateCart();
            if (cart == null) return Json(new { success = false });

            var items = _context.GioHangChiTiet.Where(g => g.MaGh == cart.MaGh).ToList();
            if (!items.Any()) return Json(new { success = false, message = "Giỏ trống" });

            // Tính tổng tiền thủ công
            double tongTien = 0;
            foreach(var item in items) {
               var thuoc = _context.Thuoc.Find(item.MaThuoc);
               if(thuoc != null) tongTien += (thuoc.Gia ?? 0) * (item.SoLuong ?? 0);
            }

            // Tạo Đơn Hàng
            var donHang = new DonHang
            {
                NgayDat = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                TongTien = tongTien,
                TrangThai = "Chờ xác nhận",
                MaKh = _context.KhachHang.FirstOrDefault(k => k.MaTk == cart.MaTk)?.MaKh
            };
            _context.DonHang.Add(donHang);
            _context.SaveChanges();

            // Chuyển từ Giỏ sang ChiTietDonHang
            foreach (var item in items)
            {
                var thuoc = _context.Thuoc.Find(item.MaThuoc);
                _context.ChiTietDonHang.Add(new ChiTietDonHang
                {
                    MaDh = donHang.MaDh,
                    MaThuoc = item.MaThuoc,
                    SoLuong = item.SoLuong,
                    DonGia = thuoc?.Gia ?? 0
                });
                // Xóa khỏi giỏ
                _context.GioHangChiTiet.Remove(item);
            }
            
            // Lưu thanh toán
            _context.ThanhToan.Add(new ThanhToan {
                MaDh = donHang.MaDh,
                PhuongThuc = phuongThuc,
                SoTien = tongTien
            });

            _context.SaveChanges();
            return Json(new { success = true, message = "Đặt hàng thành công!" });
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetOrCreateCart();
            if (cart == null) return Json(0);

            var count = _context.GioHangChiTiet
                            .Where(g => g.MaGh == cart.MaGh)
                            .Sum(g => g.SoLuong ?? 0);
            return Json(count);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int maThuoc, int soLuong)
        {
            var cart = GetOrCreateCart();
            if (cart == null) return Json(new { success = false, message = "Vui lòng đăng nhập!" });

            var item = _context.GioHangChiTiet
                           .Include(g => g.MaThuocNavigation)
                           .FirstOrDefault(g => g.MaGh == cart.MaGh && g.MaThuoc == maThuoc);

            if (item == null) return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ!" });

            item.SoLuong = soLuong;
            cart.NgayCapNhat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _context.SaveChanges();

            var thanhTien = (item.MaThuocNavigation.Gia ?? 0) * soLuong;
            var tongTien = _context.GioHangChiTiet
                            .Include(g => g.MaThuocNavigation)
                            .Where(g => g.MaGh == cart.MaGh)
                            .Sum(g => (g.MaThuocNavigation.Gia ?? 0) * (g.SoLuong ?? 0));

            return Json(new { success = true, thanhTien = thanhTien, tongTien = tongTien });
        }

        [HttpPost]
        public IActionResult RemoveItem(int maThuoc)
        {
            var cart = GetOrCreateCart();
            if (cart == null) return Json(new { success = false, message = "Vui lòng đăng nhập!" });

            var item = _context.GioHangChiTiet
                           .FirstOrDefault(g => g.MaGh == cart.MaGh && g.MaThuoc == maThuoc);

            if (item == null) return Json(new { success = false, message = "Sản phẩm không tồn tại!" });

            _context.GioHangChiTiet.Remove(item);
            cart.NgayCapNhat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _context.SaveChanges();

            var tongTien = _context.GioHangChiTiet
                            .Include(g => g.MaThuocNavigation)
                            .Where(g => g.MaGh == cart.MaGh)
                            .Sum(g => (g.MaThuocNavigation.Gia ?? 0) * (g.SoLuong ?? 0));

            return Json(new { success = true, message = "Đã xóa sản phẩm!", tongTien = tongTien });
        }
    }
}