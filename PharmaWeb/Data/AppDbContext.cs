using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PharmaWeb.Models;

namespace PharmaWeb.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaiKhoan> TaiKhoan { get; set; }
    public DbSet<KhachHang> KhachHang { get; set; }
    public DbSet<DuocSi> DuocSi { get; set; }
    public DbSet<LoaiThuoc> LoaiThuoc { get; set; }
    public DbSet<Thuoc> Thuoc { get; set; }
    public DbSet<GioHang> GioHang { get; set; }
    public DbSet<GioHangChiTiet> GioHangChiTiet { get; set; }
    public DbSet<DonHang> DonHang { get; set; }
    public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
    public DbSet<ThanhToan> ThanhToan { get; set; }
    public DbSet<LienHe> LienHe { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaCtdh);

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.MaCtdh).HasColumnName("MaCTDH");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietDonHangs).HasForeignKey(d => d.MaDh);

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.ChiTietDonHangs).HasForeignKey(d => d.MaThuoc);
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDh);

            entity.ToTable("DonHang");

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaDs).HasColumnName("MaDS");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.NgayDat).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.TongTien).HasDefaultValue(0.0);
            entity.Property(e => e.TrangThai).HasDefaultValue("Chờ xác nhận");

            entity.HasOne(d => d.MaDsNavigation).WithMany(p => p.DonHangs).HasForeignKey(d => d.MaDs);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.DonHangs).HasForeignKey(d => d.MaKh);
        });

        modelBuilder.Entity<DuocSi>(entity =>
        {
            entity.HasKey(e => e.MaDs);

            entity.ToTable("DuocSi");

            entity.Property(e => e.MaDs).HasColumnName("MaDS");
            entity.Property(e => e.MaTk).HasColumnName("MaTK");

            entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.DuocSis).HasForeignKey(d => d.MaTk);
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGh);

            entity.ToTable("GioHang");

            entity.Property(e => e.MaGh).HasColumnName("MaGH");
            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.NgayCapNhat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.GioHangs).HasForeignKey(d => d.MaTk);
        });

        modelBuilder.Entity<GioHangChiTiet>(entity =>
        {
            entity.HasKey(e => e.MaGhct);

            entity.ToTable("GioHangChiTiet");

            entity.Property(e => e.MaGhct).HasColumnName("MaGHCT");
            entity.Property(e => e.MaGh).HasColumnName("MaGH");

            entity.HasOne(d => d.MaGhNavigation).WithMany(p => p.GioHangChiTiets).HasForeignKey(d => d.MaGh);

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.GioHangChiTiets).HasForeignKey(d => d.MaThuoc);
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh);

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MaTk).HasColumnName("MaTK");

            entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.KhachHangs).HasForeignKey(d => d.MaTk);
        });

        modelBuilder.Entity<LienHe>(entity =>
        {
            entity.HasKey(e => e.MaLienHe);

            entity.ToTable("LienHe");

            entity.Property(e => e.NgayGui).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TrangThai).HasDefaultValue("Chưa xử lý");
        });

        modelBuilder.Entity<LoaiThuoc>(entity =>
        {
            entity.HasKey(e => e.MaLoai);

            entity.ToTable("LoaiThuoc");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTk);

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.TenDangNhap, "IX_TaiKhoan_TenDangNhap").IsUnique();

            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.TrangThai).HasDefaultValue(1);
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaTt);

            entity.ToTable("ThanhToan");

            entity.Property(e => e.MaTt).HasColumnName("MaTT");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.NgayThanhToan).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ThanhToans).HasForeignKey(d => d.MaDh);
        });

        modelBuilder.Entity<Thuoc>(entity =>
        {
            entity.HasKey(e => e.MaThuoc);

            entity.ToTable("Thuoc");

            entity.Property(e => e.CanKeDon).HasDefaultValue(0);

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.Thuocs).HasForeignKey(d => d.MaLoai);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
