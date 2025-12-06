using System;
using System.Collections.Generic;

namespace PharmaWeb.Models;

public partial class GioHangChiTiet
{
    public int MaGhct { get; set; }

    public int? MaGh { get; set; }

    public int? MaThuoc { get; set; }

    public int? SoLuong { get; set; }

    public virtual GioHang? MaGhNavigation { get; set; }

    public virtual Thuoc? MaThuocNavigation { get; set; }
}
