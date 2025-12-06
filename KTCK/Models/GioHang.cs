using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTCK.Models
{
    public class GioHang
    {
        QuanLyNongSanDataContext data = new QuanLyNongSanDataContext();

        public string idSP { get; set; }
        public string TenSP { get; set; }
        public string Hinh { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }

        public decimal ThanhTien
        {
            get { return SoLuong * DonGia; }
        }

        public GioHang(string id)
        {
            idSP = id.Trim();
            NongSan sp = data.NongSans.FirstOrDefault(x => x.idSP.Trim() == idSP);

            TenSP = sp.TenSP;
            DonGia = sp.DonGia;
            Hinh = sp.HinhDaiDien;
            SoLuong = 1;
        }

    }
}
