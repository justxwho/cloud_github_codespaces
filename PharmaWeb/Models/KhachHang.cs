using System;
using System.Collections.Generic;

namespace PharmaWeb.Models;

public partial class KhachHang
{
    public int MaKh { get; set; }

    public string? HoTen { get; set; }

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public string? Email { get; set; }

    public int? MaTk { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual TaiKhoan? MaTkNavigation { get; set; }
}
