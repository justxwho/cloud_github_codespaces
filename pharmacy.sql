CREATE DATABASE QL_NhaThuoc;
GO
USE QL_NhaThuoc;
GO
 
-- 1. BẢNG TÀI KHOẢN 
CREATE TABLE TaiKhoan (
    MaTK INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) UNIQUE NOT NULL,
    MatKhau NVARCHAR(255) NOT NULL,
    VaiTro NVARCHAR(20) CHECK (VaiTro IN ('Admin', 'KhachHang')) NOT NULL,
    TrangThai BIT DEFAULT 1
);

-- 2. BẢNG LOẠI THUỐC
CREATE TABLE LoaiThuoc (
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(100),
    MoTa NVARCHAR(255)
);

-- 3. BẢNG THUỐC
CREATE TABLE Thuoc (
    MaThuoc INT IDENTITY(1,1) PRIMARY KEY,
    TenThuoc NVARCHAR(200),
    HoatChat NVARCHAR(200),       
    HamLuong NVARCHAR(100),       
    DangBaoChe NVARCHAR(100),     
    Gia DECIMAL(10,2),
    MoTa NVARCHAR(MAX),
    SoLuong INT,
    Anh NVARCHAR(255),
    HanSuDung DATE,               
    SoDangKy NVARCHAR(50),        
    NhaSanXuat NVARCHAR(200),     
    NuocSanXuat NVARCHAR(100),    
    CanKeDon BIT DEFAULT 0,       
    MaLoai INT FOREIGN KEY REFERENCES LoaiThuoc(MaLoai)
);

-- 4. BẢNG DƯỢC SĨ 
CREATE TABLE DuocSi (
    MaDS INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DienThoai NVARCHAR(20),
    Email NVARCHAR(100),
    SoChungChi NVARCHAR(50),     
    MaTK INT FOREIGN KEY REFERENCES TaiKhoan(MaTK)
);

-- 5. BẢNG KHÁCH HÀNG 
CREATE TABLE KhachHang (
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100),
    DiaChi NVARCHAR(200),
    DienThoai NVARCHAR(20),
    Email NVARCHAR(100),
    MaTK INT FOREIGN KEY REFERENCES TaiKhoan(MaTK)
);

-- 6. BẢNG GIỎ HÀNG
CREATE TABLE GioHang (
    MaGH INT IDENTITY(1,1) PRIMARY KEY,
    MaTK INT FOREIGN KEY REFERENCES TaiKhoan(MaTK),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);

-- 7. BẢNG GIỎ HÀNG CHI TIẾT
CREATE TABLE GioHangChiTiet (
    MaGHCT INT IDENTITY(1,1) PRIMARY KEY,
    MaGH INT FOREIGN KEY REFERENCES GioHang(MaGH),
    MaThuoc INT FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    SoLuong INT
);

-- 8. BẢNG ĐƠN HÀNG (Thay cho HoaDon)
CREATE TABLE DonHang (
    MaDH INT IDENTITY(1,1) PRIMARY KEY,
    NgayDat DATE DEFAULT GETDATE(),
    TongTien DECIMAL(10,2),
    TrangThai NVARCHAR(50) DEFAULT N'Chờ xác nhận',
    DiaChiGiaoHang NVARCHAR(300),  
    AnhDonThuoc NVARCHAR(255),     
    GhiChu NVARCHAR(MAX),
    MaKH INT FOREIGN KEY REFERENCES KhachHang(MaKH),
    MaDS INT FOREIGN KEY REFERENCES DuocSi(MaDS) 
);

-- 9. BẢNG CHI TIẾT ĐƠN HÀNG
CREATE TABLE ChiTietDonHang (
    MaCTDH INT IDENTITY(1,1) PRIMARY KEY,
    MaDH INT FOREIGN KEY REFERENCES DonHang(MaDH),
    MaThuoc INT FOREIGN KEY REFERENCES Thuoc(MaThuoc),
    SoLuong INT,
    DonGia DECIMAL(10,2),
    CachDung NVARCHAR(500)         
);

-- 10. BẢNG THANH TOÁN
CREATE TABLE ThanhToan (
    MaTT INT IDENTITY(1,1) PRIMARY KEY,
    MaDH INT FOREIGN KEY REFERENCES DonHang(MaDH),
    PhuongThuc NVARCHAR(50),
    SoTien DECIMAL(10,2),
    NgayThanhToan DATETIME DEFAULT GETDATE()
);

-- 11. BẢNG LIÊN HỆ
CREATE TABLE LienHe (
    MaLienHe INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(15),
    TieuDe NVARCHAR(200),
    NoiDung NVARCHAR(MAX) NOT NULL, -- Câu hỏi tư vấn bệnh/thuốc
    NgayGui DATETIME DEFAULT GETDATE(),
    TrangThai NVARCHAR(50) DEFAULT N'Chưa xử lý'
);


-- Tài khoản
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro) VALUES
('admin', 'admin123', 'Admin'),        
('duocsi1', 'password', 'Admin'),      
('khach1', '123456', 'KhachHang'),
('khach2', '123456', 'KhachHang');

-- Loại thuốc
INSERT INTO LoaiThuoc (TenLoai, MoTa) VALUES 
(N'Thuốc Giảm Đau, Hạ Sốt', N'Các loại thuốc chứa Paracetamol, Ibuprofen...'),
(N'Thuốc Kháng Sinh', N'Thuốc kê đơn trị nhiễm khuẩn'),
(N'Vitamin & Khoáng Chất', N'Thực phẩm chức năng bổ sung'),
(N'Dụng Cụ Y Tế', N'Khẩu trang, nhiệt kế, máy đo huyết áp'),
(N'Thuốc Tiêu Hóa', N'Men vi sinh, dạ dày');

-- Thuốc (Dữ liệu mẫu phong phú)
INSERT INTO Thuoc (TenThuoc, HoatChat, HamLuong, DangBaoChe, Gia, MoTa, SoLuong, Anh, HanSuDung, NhaSanXuat, CanKeDon, MaLoai) VALUES
(N'Panadol Extra', N'Paracetamol, Caffeine', N'500mg', N'Viên nén', 15000, N'Giảm đau đầu, hạ sốt hiệu quả', 1000, 'panadol.jpg', '2026-12-31', N'GSK', 0, 1),
(N'Hapacol 250', N'Paracetamol', N'250mg', N'Gói bột sủi', 3000, N'Hạ sốt cho trẻ em', 500, 'hapacol.jpg', '2026-06-30', N'DHG Pharma', 0, 1),
(N'Augmentin 625mg', N'Amoxicillin, Acid Clavulanic', N'625mg', N'Viên nén', 18000, N'Kháng sinh điều trị nhiễm khuẩn đường hô hấp', 200, 'augmentin.jpg', '2025-12-20', N'GlaxoSmithKline', 1, 2), -- Cần kê đơn
(N'Vitamin C 1000mg', N'Ascorbic Acid', N'1000mg', N'Viên sủi', 60000, N'Tăng sức đề kháng', 300, 'vitaminc.jpg', '2027-01-01', N'Boston Pharma', 0, 3),
(N'Khẩu trang 4D', N'Vải không dệt', N'Không', N'Hộp 10 cái', 25000, N'Khẩu trang kháng khuẩn KF94', 2000, 'mask.jpg', '2028-01-01', N'Nam Anh', 0, 4),
(N'Men vi sinh Enterogermina', N'Bào tử lợi khuẩn', N'2 tỷ', N'Ống nhựa', 8000, N'Hỗ trợ cân bằng hệ vi sinh đường ruột', 400, 'entero.jpg', '2026-08-15', N'Sanofi', 0, 5);

-- Dược sĩ
INSERT INTO DuocSi (HoTen, NgaySinh, GioiTinh, DienThoai, Email, SoChungChi, MaTK) VALUES 
(N'Nguyễn Thu Hà', '1995-02-20', N'Nữ', '0909111222', 'ha.nguyen@pharmacy.vn', N'CCHN-12345', 1),
(N'Trần Minh Tuấn', '1990-10-10', N'Nam', '0909333444', 'tuan.tran@pharmacy.vn', N'CCHN-67890', 2);

-- Khách hàng
INSERT INTO KhachHang (HoTen, DiaChi, DienThoai, Email, MaTK) VALUES 
(N'Lê Văn An', N'Quận 1, TP.HCM', '0905111000', 'anle@gmail.com', 3),
(N'Phạm Thị Bích', N'Quận 7, TP.HCM', '0905222000', 'bichpham@gmail.com', 4);

-- Đơn hàng mẫu
INSERT INTO DonHang (NgayDat, TongTien, TrangThai, MaKH, MaDS, DiaChiGiaoHang) VALUES 
(GETDATE(), 0, N'Chờ xác nhận', 1, 1, N'123 Lê Lợi, Q1'),
(GETDATE(), 0, N'Hoàn tất', 2, 2, N'456 Nguyễn Văn Linh, Q7');

-- Chi tiết đơn hàng
INSERT INTO ChiTietDonHang (MaDH, MaThuoc, SoLuong, DonGia, CachDung) VALUES
(1, 1, 2, 15000, N'Uống sau ăn, mỗi lần 1 viên'), 
(1, 4, 1, 60000, N'Uống buổi sáng sau ăn'),     
(2, 6, 5, 8000, N'Uống trực tiếp, ngày 2 ống'); 

-- FUNCTIONS
CREATE FUNCTION fn_TinhTongTien(@MaDH INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @Tong DECIMAL(10,2);
    SELECT @Tong = SUM(SoLuong * DonGia)
    FROM ChiTietDonHang
    WHERE MaDH = @MaDH;
    RETURN ISNULL(@Tong, 0);
END;
GO

--kiểm tra thuốc có hết hạn không
CREATE FUNCTION fn_KiemTraHetHan(@MaThuoc INT)
RETURNS BIT
AS
BEGIN
    DECLARE @HanSuDung DATE;
    SELECT @HanSuDung = HanSuDung FROM Thuoc WHERE MaThuoc = @MaThuoc;
    
    IF @HanSuDung < GETDATE()
        RETURN 1; -- Đã hết hạn
    RETURN 0;     -- Còn hạn
END;
GO


--PROCEDURES

CREATE PROCEDURE sp_ThemThuoc
    @TenThuoc NVARCHAR(200),
    @HoatChat NVARCHAR(200),
    @HamLuong NVARCHAR(100),
    @DangBaoChe NVARCHAR(100),
    @Gia DECIMAL(10,2),
    @MoTa NVARCHAR(MAX),
    @SoLuong INT,
    @Anh NVARCHAR(255),
    @HanSuDung DATE,
    @NhaSanXuat NVARCHAR(200),
    @CanKeDon BIT,
    @MaLoai INT
AS
BEGIN
    INSERT INTO Thuoc (TenThuoc, HoatChat, HamLuong, DangBaoChe, Gia, MoTa, SoLuong, Anh, HanSuDung, NhaSanXuat, CanKeDon, MaLoai)
    VALUES (@TenThuoc, @HoatChat, @HamLuong, @DangBaoChe, @Gia, @MoTa, @SoLuong, @Anh, @HanSuDung, @NhaSanXuat, @CanKeDon, @MaLoai);
END;
GO


CREATE PROCEDURE sp_SuaThuoc
    @MaThuoc INT,
    @TenThuoc NVARCHAR(200),
    @HoatChat NVARCHAR(200),
    @HamLuong NVARCHAR(100),
    @DangBaoChe NVARCHAR(100),
    @Gia DECIMAL(10,2),
    @MoTa NVARCHAR(MAX),
    @SoLuong INT,
    @Anh NVARCHAR(255),
    @HanSuDung DATE,
    @NhaSanXuat NVARCHAR(200),
    @CanKeDon BIT,
    @MaLoai INT
AS
BEGIN
    UPDATE Thuoc
    SET TenThuoc = @TenThuoc,
        HoatChat = @HoatChat,
        HamLuong = @HamLuong,
        DangBaoChe = @DangBaoChe,
        Gia = @Gia,
        MoTa = @MoTa,
        SoLuong = @SoLuong,
        Anh = @Anh,
        HanSuDung = @HanSuDung,
        NhaSanXuat = @NhaSanXuat,
        CanKeDon = @CanKeDon,
        MaLoai = @MaLoai
    WHERE MaThuoc = @MaThuoc;
END;
GO

-- 3. Xóa thuốc
CREATE PROCEDURE sp_XoaThuoc
    @MaThuoc INT
AS
BEGIN
    DELETE FROM Thuoc WHERE MaThuoc = @MaThuoc;
END;
GO

-- 4. Tạo Đơn hàng mới
CREATE PROCEDURE sp_TaoDonHang
    @MaKH INT,
    @MaDS INT, 
    @DiaChiGiaoHang NVARCHAR(300),
    @AnhDonThuoc NVARCHAR(255)
AS
BEGIN
    INSERT INTO DonHang (NgayDat, TongTien, TrangThai, MaKH, MaDS, DiaChiGiaoHang, AnhDonThuoc)
    VALUES (GETDATE(), 0, N'Chờ xác nhận', @MaKH, @MaDS, @DiaChiGiaoHang, @AnhDonThuoc);
END;
GO

-- 5. Thêm chi tiết đơn hàng
CREATE PROCEDURE sp_ThemChiTietDonHang
    @MaDH INT,
    @MaThuoc INT,
    @SoLuong INT,
    @DonGia DECIMAL(10,2),
    @CachDung NVARCHAR(500)
AS
BEGIN
    -- Kiểm tra thuốc hết hạn
    IF dbo.fn_KiemTraHetHan(@MaThuoc) = 1
    BEGIN
        RAISERROR(N'Lỗi: Thuốc này đã hết hạn sử dụng, không thể bán!', 16, 1);
        RETURN;
    END

    INSERT INTO ChiTietDonHang (MaDH, MaThuoc, SoLuong, DonGia, CachDung)
    VALUES (@MaDH, @MaThuoc, @SoLuong, @DonGia, @CachDung);

    -- Cập nhật tổng tiền đơn hàng
    DECLARE @TongTien DECIMAL(10,2);
    SELECT @TongTien = dbo.fn_TinhTongTien(@MaDH);
    UPDATE DonHang SET TongTien = @TongTien WHERE MaDH = @MaDH;
END;
GO


-- TRIGGERS 

CREATE TRIGGER trg_GiamSoLuongSauKhiBan
ON ChiTietDonHang
AFTER INSERT
AS
BEGIN
    UPDATE t
    SET t.SoLuong = t.SoLuong - i.SoLuong
    FROM Thuoc t
    JOIN inserted i ON t.MaThuoc = i.MaThuoc;
END;
GO

-- 2. Cập nhật tổng tiền khi sửa chi tiết đơn
CREATE TRIGGER trg_CapNhatTongTienDonHang
ON ChiTietDonHang
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @MaDH INT;

    SELECT TOP 1 @MaDH = COALESCE(i.MaDH, d.MaDH)
    FROM inserted i
    FULL JOIN deleted d ON i.MaCTDH = d.MaCTDH;

    UPDATE DonHang
    SET TongTien = dbo.fn_TinhTongTien(@MaDH)
    WHERE MaDH = @MaDH;
END;
GO