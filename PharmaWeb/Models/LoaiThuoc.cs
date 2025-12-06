using System;
using System.Collections.Generic;

namespace PharmaWeb.Models;

public partial class LoaiThuoc
{
    public int MaLoai { get; set; }

    public string? TenLoai { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<Thuoc> Thuocs { get; set; } = new List<Thuoc>();
}
