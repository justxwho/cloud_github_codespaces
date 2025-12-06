using System;
using System.Collections.Generic;

namespace PharmaWeb.Models;

public partial class GioHang
{
    public int MaGh { get; set; }

    public int? MaTk { get; set; }

    public string? NgayCapNhat { get; set; }

    public virtual ICollection<GioHangChiTiet> GioHangChiTiets { get; set; } = new List<GioHangChiTiet>();

    public virtual TaiKhoan? MaTkNavigation { get; set; }
}
