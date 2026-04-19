using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LienHe",
                columns: table => new
                {
                    MaLienHe = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HoTen = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    SoDienThoai = table.Column<string>(type: "TEXT", nullable: true),
                    TieuDe = table.Column<string>(type: "TEXT", nullable: true),
                    NoiDung = table.Column<string>(type: "TEXT", nullable: false),
                    NgayGui = table.Column<string>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    TrangThai = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "Chưa xử lý")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LienHe", x => x.MaLienHe);
                });

            migrationBuilder.CreateTable(
                name: "LoaiThuoc",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenLoai = table.Column<string>(type: "TEXT", nullable: true),
                    MoTa = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiThuoc", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    MaTK = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenDangNhap = table.Column<string>(type: "TEXT", nullable: false),
                    MatKhau = table.Column<string>(type: "TEXT", nullable: false),
                    VaiTro = table.Column<string>(type: "TEXT", nullable: false),
                    TrangThai = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.MaTK);
                });

            migrationBuilder.CreateTable(
                name: "Thuoc",
                columns: table => new
                {
                    MaThuoc = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenThuoc = table.Column<string>(type: "TEXT", nullable: true),
                    HoatChat = table.Column<string>(type: "TEXT", nullable: true),
                    HamLuong = table.Column<string>(type: "TEXT", nullable: true),
                    DangBaoChe = table.Column<string>(type: "TEXT", nullable: true),
                    Gia = table.Column<double>(type: "REAL", nullable: true),
                    MoTa = table.Column<string>(type: "TEXT", nullable: true),
                    SoLuong = table.Column<int>(type: "INTEGER", nullable: true),
                    Anh = table.Column<string>(type: "TEXT", nullable: true),
                    HanSuDung = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    SoDangKy = table.Column<string>(type: "TEXT", nullable: true),
                    NhaSanXuat = table.Column<string>(type: "TEXT", nullable: true),
                    NuocSanXuat = table.Column<string>(type: "TEXT", nullable: true),
                    CanKeDon = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    MaLoai = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thuoc", x => x.MaThuoc);
                    table.ForeignKey(
                        name: "FK_Thuoc_LoaiThuoc_MaLoai",
                        column: x => x.MaLoai,
                        principalTable: "LoaiThuoc",
                        principalColumn: "MaLoai");
                });

            migrationBuilder.CreateTable(
                name: "DuocSi",
                columns: table => new
                {
                    MaDS = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HoTen = table.Column<string>(type: "TEXT", nullable: true),
                    NgaySinh = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    GioiTinh = table.Column<string>(type: "TEXT", nullable: true),
                    DienThoai = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    SoChungChi = table.Column<string>(type: "TEXT", nullable: true),
                    MaTK = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuocSi", x => x.MaDS);
                    table.ForeignKey(
                        name: "FK_DuocSi_TaiKhoan_MaTK",
                        column: x => x.MaTK,
                        principalTable: "TaiKhoan",
                        principalColumn: "MaTK");
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    MaGH = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaTK = table.Column<int>(type: "INTEGER", nullable: true),
                    NgayCapNhat = table.Column<string>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.MaGH);
                    table.ForeignKey(
                        name: "FK_GioHang_TaiKhoan_MaTK",
                        column: x => x.MaTK,
                        principalTable: "TaiKhoan",
                        principalColumn: "MaTK");
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKH = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HoTen = table.Column<string>(type: "TEXT", nullable: true),
                    DiaChi = table.Column<string>(type: "TEXT", nullable: true),
                    DienThoai = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    MaTK = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.MaKH);
                    table.ForeignKey(
                        name: "FK_KhachHang_TaiKhoan_MaTK",
                        column: x => x.MaTK,
                        principalTable: "TaiKhoan",
                        principalColumn: "MaTK");
                });

            migrationBuilder.CreateTable(
                name: "GioHangChiTiet",
                columns: table => new
                {
                    MaGHCT = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaGH = table.Column<int>(type: "INTEGER", nullable: true),
                    MaThuoc = table.Column<int>(type: "INTEGER", nullable: true),
                    SoLuong = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangChiTiet", x => x.MaGHCT);
                    table.ForeignKey(
                        name: "FK_GioHangChiTiet_GioHang_MaGH",
                        column: x => x.MaGH,
                        principalTable: "GioHang",
                        principalColumn: "MaGH");
                    table.ForeignKey(
                        name: "FK_GioHangChiTiet_Thuoc_MaThuoc",
                        column: x => x.MaThuoc,
                        principalTable: "Thuoc",
                        principalColumn: "MaThuoc");
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    MaDH = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NgayDat = table.Column<DateOnly>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    TongTien = table.Column<double>(type: "REAL", nullable: true, defaultValue: 0.0),
                    TrangThai = table.Column<string>(type: "TEXT", nullable: true, defaultValue: "Chờ xác nhận"),
                    DiaChiGiaoHang = table.Column<string>(type: "TEXT", nullable: true),
                    AnhDonThuoc = table.Column<string>(type: "TEXT", nullable: true),
                    GhiChu = table.Column<string>(type: "TEXT", nullable: true),
                    MaKH = table.Column<int>(type: "INTEGER", nullable: true),
                    MaDS = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.MaDH);
                    table.ForeignKey(
                        name: "FK_DonHang_DuocSi_MaDS",
                        column: x => x.MaDS,
                        principalTable: "DuocSi",
                        principalColumn: "MaDS");
                    table.ForeignKey(
                        name: "FK_DonHang_KhachHang_MaKH",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHang",
                columns: table => new
                {
                    MaCTDH = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaDH = table.Column<int>(type: "INTEGER", nullable: true),
                    MaThuoc = table.Column<int>(type: "INTEGER", nullable: true),
                    SoLuong = table.Column<int>(type: "INTEGER", nullable: true),
                    DonGia = table.Column<double>(type: "REAL", nullable: true),
                    CachDung = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHang", x => x.MaCTDH);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_DonHang_MaDH",
                        column: x => x.MaDH,
                        principalTable: "DonHang",
                        principalColumn: "MaDH");
                    table.ForeignKey(
                        name: "FK_ChiTietDonHang_Thuoc_MaThuoc",
                        column: x => x.MaThuoc,
                        principalTable: "Thuoc",
                        principalColumn: "MaThuoc");
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    MaTT = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaDH = table.Column<int>(type: "INTEGER", nullable: true),
                    PhuongThuc = table.Column<string>(type: "TEXT", nullable: true),
                    SoTien = table.Column<double>(type: "REAL", nullable: true),
                    NgayThanhToan = table.Column<string>(type: "TEXT", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.MaTT);
                    table.ForeignKey(
                        name: "FK_ThanhToan_DonHang_MaDH",
                        column: x => x.MaDH,
                        principalTable: "DonHang",
                        principalColumn: "MaDH");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaDH",
                table: "ChiTietDonHang",
                column: "MaDH");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHang_MaThuoc",
                table: "ChiTietDonHang",
                column: "MaThuoc");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaDS",
                table: "DonHang",
                column: "MaDS");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaKH",
                table: "DonHang",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_DuocSi_MaTK",
                table: "DuocSi",
                column: "MaTK");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_MaTK",
                table: "GioHang",
                column: "MaTK");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiet_MaGH",
                table: "GioHangChiTiet",
                column: "MaGH");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiet_MaThuoc",
                table: "GioHangChiTiet",
                column: "MaThuoc");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_MaTK",
                table: "KhachHang",
                column: "MaTK");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_TenDangNhap",
                table: "TaiKhoan",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_MaDH",
                table: "ThanhToan",
                column: "MaDH");

            migrationBuilder.CreateIndex(
                name: "IX_Thuoc_MaLoai",
                table: "Thuoc",
                column: "MaLoai");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHang");

            migrationBuilder.DropTable(
                name: "GioHangChiTiet");

            migrationBuilder.DropTable(
                name: "LienHe");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "Thuoc");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "LoaiThuoc");

            migrationBuilder.DropTable(
                name: "DuocSi");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "TaiKhoan");
        }
    }
}
