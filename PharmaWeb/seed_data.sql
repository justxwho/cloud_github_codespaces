-- 1. TÀI KHOẢN
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThai) VALUES
('admin', 'admin123', 'Admin', 1),        
('duocsi1', 'password', 'Admin', 1),      
('khach1', '123456', 'KhachHang', 1),
('khach2', '123456', 'KhachHang', 1);

-- 2. LOẠI THUỐC
INSERT INTO LoaiThuoc (TenLoai, MoTa) VALUES 
('Thuốc Giảm Đau, Hạ Sốt', 'Các loại thuốc chứa Paracetamol, Ibuprofen...'),
('Thuốc Kháng Sinh', 'Thuốc kê đơn trị nhiễm khuẩn'),
('Vitamin & Khoáng Chất', 'Thực phẩm chức năng bổ sung'),
('Dụng Cụ Y Tế', 'Khẩu trang, nhiệt kế, máy đo huyết áp'),
('Thuốc Tiêu Hóa', 'Men vi sinh, dạ dày');

-- 3. THUỐC
INSERT INTO Thuoc (TenThuoc, HoatChat, HamLuong, DangBaoChe, Gia, MoTa, SoLuong, Anh, HanSuDung, NhaSanXuat, NuocSanXuat, SoDangKy, CanKeDon, MaLoai) VALUES
('Panadol Extra', 'Paracetamol, Caffeine', '500mg', 'Viên nén', 15000, 'Giảm đau đầu, hạ sốt hiệu quả', 1000, 'panadol.jpg', '2026-12-31', 'GSK', 'Việt Nam', 'VD-001', 0, 1),
('Hapacol 250', 'Paracetamol', '250mg', 'Gói bột sủi', 3000, 'Hạ sốt cho trẻ em', 500, 'hapacol.jpg', '2026-06-30', 'DHG Pharma', 'Việt Nam', 'VD-002', 0, 1),
('Augmentin 625mg', 'Amoxicillin, Acid Clavulanic', '625mg', 'Viên nén', 18000, 'Kháng sinh điều trị nhiễm khuẩn đường hô hấp', 200, 'augmentin.jpg', '2025-12-20', 'GlaxoSmithKline', 'Anh', 'VD-003', 1, 2),
('Amoxicillin 500mg', 'Amoxicillin', '500mg', 'Viên nang', 12000, 'Kháng sinh phổ rộng', 300, 'Amoxicillin.jpg', '2026-03-15', 'Boston', 'Việt Nam', 'VD-004', 1, 2),
('Azithromycin 250mg', 'Azithromycin', '250mg', 'Viên nang', 25000, 'Kháng sinh điều trị nhiễm khuẩn', 150, 'azithromycin.jpg', '2026-08-20', 'Teva', 'Israel', 'VD-005', 1, 2),
('Vitamin C 1000mg', 'Ascorbic Acid', '1000mg', 'Viên sủi', 60000, 'Tăng sức đề kháng', 300, 'vitaminc.jpg', '2027-01-01', 'Boston Pharma', 'Việt Nam', 'VD-006', 0, 3),
('Vitamin D3 5000IU', 'Cholecalciferol', '5000IU', 'Viên nang mềm', 180000, 'Bổ sung vitamin D3, tốt cho xương', 200, 'vitamind3.jpg', '2027-05-15', 'Nature Made', 'Mỹ', 'VD-007', 0, 3),
('Omega 3 Fish Oil', 'DHA, EPA', '1000mg', 'Viên nang mềm', 250000, 'Hỗ trợ tim mạch và não bộ', 150, 'omega3.jpg', '2027-02-28', 'Nordic Naturals', 'Mỹ', 'VD-008', 0, 3),
('Khẩu trang 4D', 'Vải không dệt', 'Không', 'Hộp 10 cái', 25000, 'Khẩu trang kháng khuẩn KF94', 2000, 'mask.jpg', '2028-01-01', 'Nam Anh', 'Việt Nam', 'VD-009', 0, 4),
('Nhiệt kế điện tử', 'Thiết bị y tế', 'Không', 'Cái', 150000, 'Nhiệt kế đo nhanh, chính xác', 100, 'thermometer.jpg', '2030-01-01', 'Omron', 'Nhật Bản', 'VD-010', 0, 4),
('Men vi sinh Enterogermina', 'Bào tử lợi khuẩn', '2 tỷ', 'Ống nhựa', 8000, 'Hỗ trợ cân bằng hệ vi sinh đường ruột', 400, 'entero.jpg', '2026-08-15', 'Sanofi', 'Italia', 'VD-011', 0, 5),
('Gaviscon Suspension', 'Sodium Alginate', '10ml', 'Gói siro', 5000, 'Giảm ợ nóng, trào ngược dạ dày', 500, 'gaviscon.jpg', '2026-11-30', 'Reckitt Benckiser', 'Anh', 'VD-012', 0, 5),
('Cetirizin 10mg', 'Cetirizin HCl', '10mg', 'Viên nén', 2500, 'Điều trị dị ứng, viêm mũi dị ứng', 600, 'cetirizin.jpg', '2026-09-10', 'Imexpharm', 'Việt Nam', 'VD-013', 0, 1),
('Ibuprofen 400mg', 'Ibuprofen', '400mg', 'Viên nén', 8000, 'Giảm đau, chống viêm', 400, 'ibuprofen.jpg', '2026-07-25', 'Abbott', 'Việt Nam', 'VD-014', 0, 1);

-- 4. DƯỢC SĨ
INSERT INTO DuocSi (HoTen, NgaySinh, GioiTinh, DienThoai, Email, SoChungChi, MaTK) VALUES 
('Nguyễn Thu Hà', '1995-02-20', 'Nữ', '0909111222', 'ha.nguyen@pharmacy.vn', 'CCHN-12345', 2),
('Trần Minh Tuấn', '1990-10-10', 'Nam', '0909333444', 'tuan.tran@pharmacy.vn', 'CCHN-67890', 2);

-- 5. KHÁCH HÀNG
INSERT INTO KhachHang (HoTen, DiaChi, DienThoai, Email, MaTK) VALUES 
('Lê Văn An', 'Quận 1, TP.HCM', '0905111000', 'anle@gmail.com', 3),
('Phạm Thị Bích', 'Quận 7, TP.HCM', '0905222000', 'bichpham@gmail.com', 4);

-- 6. ĐƠN HÀNG MẪU
INSERT INTO DonHang (NgayDat, TongTien, TrangThai, MaKH, MaDS, DiaChiGiaoHang) VALUES 
('2025-12-07', 90000, 'Chờ xác nhận', 1, 1, '123 Lê Lợi, Q1'),
('2025-12-06', 40000, 'Hoàn tất', 2, 2, '456 Nguyễn Văn Linh, Q7');

-- 7. CHI TIẾT ĐƠN HÀNG
INSERT INTO ChiTietDonHang (MaDH, MaThuoc, SoLuong, DonGia, CachDung) VALUES
(1, 1, 2, 15000, 'Uống sau ăn, mỗi lần 1 viên'), 
(1, 6, 1, 60000, 'Uống buổi sáng sau ăn'),     
(2, 11, 5, 8000, 'Uống trực tiếp, ngày 2 ống');
