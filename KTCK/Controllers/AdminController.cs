using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KTCK.Models;

namespace KTCK.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        QuanLyNongSanDataContext data = new QuanLyNongSanDataContext();

        // Danh sách sản phẩm
        public ActionResult SanPham()
        {
            var list = data.NongSans.ToList();
            return View(list);
        }

        // GET: Thêm sản phẩm
        public ActionResult ThemSanPham()
        {
            ViewBag.Loai = new SelectList(data.Loais.ToList(), "MaLoai", "TenLoai");
            return View();
        }

        // POST: Thêm sản phẩm
        [HttpPost]
        public ActionResult ThemSanPham(NongSan model)
        {
            if (string.IsNullOrEmpty(model.idSP) || string.IsNullOrEmpty(model.TenSP))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                ViewBag.Loai = new SelectList(data.Loais.ToList(), "MaLoai", "TenLoai");
                return View(model);
            }

            // Kiểm tra ID sản phẩm trùng
            var kt = data.NongSans.FirstOrDefault(x => x.idSP.Trim() == model.idSP.Trim());
            if (kt != null)
            {
                ViewBag.Error = "Mã sản phẩm đã tồn tại!";
                ViewBag.Loai = new SelectList(data.Loais.ToList(), "MaLoai", "TenLoai");
                return View(model);
            }

            // Thêm vào DB
            data.NongSans.InsertOnSubmit(model);
            data.SubmitChanges();

            TempData["Success"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("SanPham");
        }
    }
}
