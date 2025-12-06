using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTCK.Models;

namespace KTCK.Controllers
{
    public class HomeController : Controller
    {
        QuanLyNongSanDataContext data = new QuanLyNongSanDataContext();

        /////ViewBag truyền tham số
        public ActionResult Index()
        {
            List<string> thongtin = new List<string>
            {
                "Họ và tên: Trần Võ ĐÌnh Trung","MSSV:2001231012","Lớp: 14DHTH11"
            };
            ViewBag.thongtinsv = thongtin;
             return View();
        }

       ////////Hiển thị sản phẩm
        public ActionResult HTSP()
        {
            List<NongSan> dsNS = data.NongSans.Take(30).ToList();
            var dsnongsan = data.NongSans.ToList();
            return View(dsNS);
        }
        ///SẩN PHẨM THEO LOẠ
        public ActionResult MenuLoai(int madm)
        {
            var ds = data.Loais
                         .Where(x => x.MaDanhMuc == madm)
                         .ToList();

            return PartialView("_MenuLoai", ds);
        }

        public ActionResult SPTheoLoai(int id)
        {
            var sp = data.NongSans.Where(x => x.MaLoaiSP == id).ToList();
            return View(sp);
        }



        ///////Đăng nhập
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(string txtName, string txtPass)
        {
            var kh = data.KhachHangs
                         .FirstOrDefault(x => x.TenKH == txtName && x.MatKhau == txtPass);
            if (kh == null) { ViewBag.ThongBao = "Sai tài khoản hoặc mật khẩu"; return View(); }

            Session["kh"] = kh;
            Session["UserName"] = kh.TenKH;   // layout dùng biến này
            return RedirectToAction("HTSP");
        }

      
      //////Tìm kiếm
        public ActionResult SearchForm()
        {
            ViewBag.Loai = data.Loais.ToList();   // lấy loại từ CSDL
            return View();
        }
        [HttpGet]
        public ActionResult Search(string keyword, int? maloai, bool? timMoTa)
        {
            var query = data.NongSans.AsQueryable(); // lấy bảng NongSan

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.TenSP.Contains(keyword));
            }

            if (maloai.HasValue)
            {
                query = query.Where(x => x.MaLoaiSP == maloai);
            }

            if (timMoTa == true)
            {
                query = query.Where(x => x.MoTa.Contains(keyword));
            }

            return View("SearchResult", query.ToList());
        }

        //////Chi tiết
        public ActionResult ChiTiet(string id)
        {
            if (id == null) return HttpNotFound();
            id = id.Trim();

            // Lấy sản phẩm
            var sp = data.NongSans.FirstOrDefault(x => x.idSP.Trim() == id);
            if (sp == null) return HttpNotFound();

            // Lấy tỉnh giao hàng
            var giaoHang = from gh in data.GiaoHangs
                           join t in data.Tinhs on gh.MaTinh equals t.MaTinh
                           where gh.MaSP.Trim() == id
                           select t.TenTinh;

            ViewBag.TinhGiaoHang = giaoHang.ToList();

            // Sản phẩm liên quan
            ViewBag.LienQuan = data.NongSans
                .Where(x => x.MaLoaiSP == sp.MaLoaiSP && x.idSP != sp.idSP)
                .Take(4)
                .ToList();

            return View(sp);
        }

        //////Đăng ký
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(KhachHang model, string ConfirmPass)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(model.TenKH) ||
                string.IsNullOrEmpty(model.MatKhau) ||
                string.IsNullOrEmpty(ConfirmPass))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }

            // Kiểm tra mật khẩu nhập lại
            if (model.MatKhau != ConfirmPass)
            {
                ViewBag.Error = "Mật khẩu nhập lại không khớp!";
                return View();
            }

            // Kiểm tra trùng username / số điện thoại
            var kt = data.KhachHangs.FirstOrDefault(x => x.TenKH == model.TenKH);
            if (kt != null)
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            // Tự tăng idKH
            model.idKH = data.KhachHangs.Count() + 1;

            // Lưu DB
            data.KhachHangs.InsertOnSubmit(model);
            data.SubmitChanges();

            ViewBag.Success = "Đăng ký thành công! Hãy đăng nhập.";
            return RedirectToAction("DangNhap");
        }

        ///// Lấy giỏ hàng trong Session
        public List<GioHang> LayGioHang()
        {
            List<GioHang> gio = Session["GioHang"] as List<GioHang>;
            if (gio == null)
            {
                gio = new List<GioHang>();
                Session["GioHang"] = gio;
            }
            return gio;
        }

        ///// Thêm sản phẩm vào giỏ
        public ActionResult ThemGioHang(string id)
        {
            id = id.Trim();
            List<GioHang> gio = LayGioHang();
            GioHang sp = gio.FirstOrDefault(x => x.idSP == id);

            if (sp == null)
            {
                gio.Add(new GioHang(id));
            }
            else
            {
                sp.SoLuong++;
            }

            return RedirectToAction("HTSP");  
        }


        //// Xem giỏ hàng
        public ActionResult XemGioHang()
        {
            List<GioHang> gio = LayGioHang();
            ViewBag.TongTien = gio.Sum(x => x.ThanhTien);
            return View(gio);
        }

        /// Xóa sản phẩm
        public ActionResult XoaItem(string id)
        {
            List<GioHang> gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.idSP == id);
            if (item != null) gio.Remove(item);
            return RedirectToAction("XemGioHang");
        }

        //// Cập nhật số lượng
        [HttpPost]
        public ActionResult CapNhat(string id, int soluong)
        {
            List<GioHang> gio = LayGioHang();
            var item = gio.FirstOrDefault(x => x.idSP == id);
            if (item != null) item.SoLuong = soluong;

            return RedirectToAction("XemGioHang");
        }

        public ActionResult XacNhanDatHang()
        {
            if (Session["kh"] == null)
                return RedirectToAction("DangNhap");

            var gio = LayGioHang();
            if (gio.Count == 0)
                return RedirectToAction("HTSP");

            return View(gio);
        }
        [HttpPost]
        public ActionResult LuuDonHang(string HinhThucGiao, string GhiChu)
        {
            var kh = Session["kh"] as KhachHang;
            if (kh == null) return RedirectToAction("DangNhap");

            var gio = LayGioHang();
            if (gio.Count == 0) return RedirectToAction("XemGioHang");

            // Tạo đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.idDDH = data.DonDatHangs.Count() + 1;
            ddh.idKhachHang = kh.idKH;
            ddh.NgayDatHang = DateTime.Now;
            ddh.HinhThucGiaoHang = HinhThucGiao;
            ddh.GhiChu = GhiChu;

            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();

            // Chi tiết đơn hàng
            foreach (var item in gio)
            {
                var sp = data.NongSans.FirstOrDefault(x => x.idSP.Trim() == item.idSP.Trim());
                if (sp == null) continue;

                ChiTietDDH ct = new ChiTietDDH();
                ct.idDDH = ddh.idDDH;
                ct.idSP = sp.idSP;  // lưu idSP đúng nguyên bản trong DB
                ct.SoLuong = item.SoLuong;

                data.ChiTietDDHs.InsertOnSubmit(ct);
            }

            try
            {
                data.SubmitChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return Content("Lỗi: " + ex.Message);
            }

            // Xóa giỏ
            Session["GioHang"] = null;

            TempData["Success"] = "Đặt hàng thành công!";
            return RedirectToAction("HTSP");
        }





    }
}