using System;
using System.Collections.Generic;

namespace PharmaWeb.Models;

public partial class DuocSi
{
    public int MaDs { get; set; }

    public string? HoTen { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? DienThoai { get; set; }

    public string? Email { get; set; }

    public string? SoChungChi { get; set; }

    public int? MaTk { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual TaiKhoan? MaTkNavigation { get; set; }
}
