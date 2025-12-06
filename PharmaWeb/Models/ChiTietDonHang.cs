using System;
using System.Collections.Generic;

namespace PharmaWeb.Models;

public partial class ChiTietDonHang
{
    public int MaCtdh { get; set; }

    public int? MaDh { get; set; }

    public int? MaThuoc { get; set; }

    public int? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public string? CachDung { get; set; }

    public virtual DonHang? MaDhNavigation { get; set; }

    public virtual Thuoc? MaThuocNavigation { get; set; }
}
