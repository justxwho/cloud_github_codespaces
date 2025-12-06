using System;
using System.Collections.Generic;

namespace PharmaWeb.Models;

public partial class Thuoc
{
    public int MaThuoc { get; set; }

    public string? TenThuoc { get; set; }

    public string? HoatChat { get; set; }

    public string? HamLuong { get; set; }

    public string? DangBaoChe { get; set; }

    public double? Gia { get; set; }

    public string? MoTa { get; set; }

    public int? SoLuong { get; set; }

    public string? Anh { get; set; }

    public DateOnly? HanSuDung { get; set; }

    public string? SoDangKy { get; set; }

    public string? NhaSanXuat { get; set; }

    public string? NuocSanXuat { get; set; }

    public int? CanKeDon { get; set; }

    public int? MaLoai { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<GioHangChiTiet> GioHangChiTiets { get; set; } = new List<GioHangChiTiet>();

    public virtual LoaiThuoc? MaLoaiNavigation { get; set; }
}
