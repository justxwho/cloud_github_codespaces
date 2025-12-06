using System;
using System.Collections.Generic;

namespace PharmaWeb.Models;

public partial class ThanhToan
{
    public int MaTt { get; set; }

    public int? MaDh { get; set; }

    public string? PhuongThuc { get; set; }

    public double? SoTien { get; set; }

    public string? NgayThanhToan { get; set; }

    public virtual DonHang? MaDhNavigation { get; set; }
}
