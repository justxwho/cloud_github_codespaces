using System;
using System.Collections.Generic;

namespace PharmaWeb.Models;

public partial class DonHang
{
    public int MaDh { get; set; }

    public DateOnly? NgayDat { get; set; }

    public double? TongTien { get; set; }

    public string? TrangThai { get; set; }

    public string? DiaChiGiaoHang { get; set; }

    public string? AnhDonThuoc { get; set; }

    public string? GhiChu { get; set; }

    public int? MaKh { get; set; }

    public int? MaDs { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual DuocSi? MaDsNavigation { get; set; }

    public virtual KhachHang? MaKhNavigation { get; set; }

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}
