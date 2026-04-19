using Microsoft.AspNetCore.Mvc;
using PharmaWeb.Models; 
using PharmaWeb.Data;   
using System.Linq;

namespace PharmaWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // ======================
        // LOGIN
        // ======================
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string tendangnhap, string matkhau, string returnUrl = null)
        {
            // Tìm tài khoản
            var tk = _context.TaiKhoan
                .FirstOrDefault(t => t.TenDangNhap == tendangnhap && t.MatKhau == matkhau);

            // ❌ Sai tài khoản hoặc mật khẩu
            if (tk == null)
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
                return View();
            }

            // 🔒 TÀI KHOẢN BỊ KHÓA
            if (tk.TrangThai == 0)
            {
                ViewBag.Error = "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên!";
                return View();
            }

            // ✅ OK → LƯU SESSION
            HttpContext.Session.SetString("MaTK", tk.MaTk.ToString());
            HttpContext.Session.SetString("Role", tk.VaiTro);
            HttpContext.Session.SetString("UserName", tk.TenDangNhap);

            // ADMIN
            if (tk.VaiTro == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            // KHÁCH HÀNG
            var kh = _context.KhachHang.FirstOrDefault(k => k.MaTk == tk.MaTk);
            if (kh != null)
            {
                HttpContext.Session.SetString("UserName", kh.HoTen);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // ======================
        // LOGOUT
        // ======================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ======================
        // REGISTER
        // ======================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string hoten, string diachi, string dienthoai, string email, string tendangnhap, string matkhau)
        {
            if (_context.TaiKhoan.Any(t => t.TenDangNhap == tendangnhap))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            // 1. Tạo tài khoản
            var tk = new TaiKhoan
            {
                TenDangNhap = tendangnhap,
                MatKhau = matkhau,
                VaiTro = "KhachHang",
                TrangThai = 1 // MẶC ĐỊNH HOẠT ĐỘNG
            };
            _context.TaiKhoan.Add(tk);
            _context.SaveChanges();

            // 2. Tạo khách hàng
            var kh = new KhachHang
            {
                HoTen = hoten,
                DiaChi = diachi,
                DienThoai = dienthoai,
                Email = email,
                MaTk = tk.MaTk
            };
            _context.KhachHang.Add(kh);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}
