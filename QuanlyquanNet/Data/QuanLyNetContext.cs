using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuanlyquanNet.Data;

public partial class QuanLyNetContext : DbContext
{
    public QuanLyNetContext()
    {
    }

    public QuanLyNetContext(DbContextOptions<QuanLyNetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DichVu> DichVus { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GoiNap> GoiNaps { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhachHangNhiemVu> KhachHangNhiemVus { get; set; }

    public virtual DbSet<KhuVuc> KhuVucs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<LichSuDangNhap> LichSuDangNhaps { get; set; }

    public virtual DbSet<LichSuNapTien> LichSuNapTiens { get; set; }

    public virtual DbSet<MayTinh> MayTinhs { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<NhiemVu> NhiemVus { get; set; }

    public virtual DbSet<PhanThuongDaNhan> PhanThuongDaNhans { get; set; }

    public virtual DbSet<PhienChoi> PhienChois { get; set; }

    public virtual DbSet<PhuThuong> PhuThuongs { get; set; }

    public virtual DbSet<ThongBao> ThongBaos { get; set; }

    public virtual DbSet<ThongKe> ThongKes { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<VatPham> VatPhams { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-NNTSRGH;Initial Catalog=QuanLyNet;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DichVu>(entity =>
        {
            entity.HasKey(e => e.MaDichVu).HasName("PK__DichVu__C0E6DE8FFDE2DE7A");

            entity.ToTable("DichVu");

            entity.HasIndex(e => e.TenDichVu, "UQ__DichVu__A77D06899A292DA0").IsUnique();

            entity.Property(e => e.Gia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.LoaiDichVu).HasMaxLength(50);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDichVu).HasMaxLength(100);
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonHang__129584ADE2FFC440");

            entity.ToTable("DonHang");

            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThongTinViTri).HasMaxLength(100);
            entity.Property(e => e.TongTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang xử lý");

            entity.HasOne(d => d.MaDichVuNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaDichVu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaDichV__7A672E12");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaKhach__797309D9");

            entity.HasOne(d => d.MaMayTinhNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaMayTinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaMayTi__7B5B524B");
        });

        modelBuilder.Entity<GoiNap>(entity =>
        {
            entity.HasKey(e => e.MaGoiNap).HasName("PK__GoiNap__994CA54CDEA0CCAE");

            entity.ToTable("GoiNap");

            entity.HasIndex(e => e.TenGoi, "UQ__GoiNap__32A249F68957F730").IsUnique();

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TenGoi).HasMaxLength(50);

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.GoiNaps)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__GoiNap__MaKhuyen__02084FDA");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E5560B7099");

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.SoDienThoai, "UQ__KhachHan__0389B7BD579C912D").IsUnique();

            entity.HasIndex(e => e.TenDangNhap, "UQ__KhachHan__55F68FC056E75FAB").IsUnique();

            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.LoaiTaiKhoan)
                .HasMaxLength(20)
                .HasDefaultValue("Thường");
            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.SoDu)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);

            entity.HasOne(d => d.TenDangNhapNavigation).WithOne(p => p.KhachHang)
                .HasPrincipalKey<NguoiDung>(p => p.TenDangNhap)
                .HasForeignKey<KhachHang>(d => d.TenDangNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhachHang__TenDa__46E78A0C");
        });

        modelBuilder.Entity<KhachHangNhiemVu>(entity =>
        {
            entity.HasKey(e => new { e.MaKhachHang, e.MaNhiemVu }).HasName("PK__KhachHan__6E47725721621C51");

            entity.ToTable("KhachHang_NhiemVu");

            entity.Property(e => e.DiemNhanDuoc).HasDefaultValue(0);
            entity.Property(e => e.NgayThamGia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThoiGianHoanThanh).HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Chưa hoàn thành");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.KhachHangNhiemVus)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhachHang__MaKha__2180FB33");

            entity.HasOne(d => d.MaNhiemVuNavigation).WithMany(p => p.KhachHangNhiemVus)
                .HasForeignKey(d => d.MaNhiemVu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhachHang__MaNhi__22751F6C");
        });

        modelBuilder.Entity<KhuVuc>(entity =>
        {
            entity.HasKey(e => e.MaKhuVuc).HasName("PK__KhuVuc__0676EB83E49D3BBC");

            entity.ToTable("KhuVuc");

            entity.HasIndex(e => e.TenKhuVuc, "UQ__KhuVuc__258A8CB3A2F38C3C").IsUnique();

            entity.Property(e => e.GiaMoiGio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenKhuVuc).HasMaxLength(100);
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__6F56B3BD6DD73996");

            entity.ToTable("KhuyenMai");

            entity.HasIndex(e => e.TenKhuyenMai, "UQ__KhuyenMa__A956B87C9D232BFC").IsUnique();

            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhanTramTang)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TenKhuyenMai).HasMaxLength(100);
        });

        modelBuilder.Entity<LichSuDangNhap>(entity =>
        {
            entity.HasKey(e => e.MaDangNhap).HasName("PK__LichSuDa__C869B8C09326DF38");

            entity.ToTable("LichSuDangNhap");

            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.ThoiGianDangNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.VaiTro).HasMaxLength(50);

            entity.HasOne(d => d.TenDangNhapNavigation).WithMany(p => p.LichSuDangNhaps)
                .HasPrincipalKey(p => p.TenDangNhap)
                .HasForeignKey(d => d.TenDangNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuDan__TenDa__3F466844");
        });

        modelBuilder.Entity<LichSuNapTien>(entity =>
        {
            entity.HasKey(e => e.MaNapTien).HasName("PK__LichSuNa__86747C76DF801AD7");

            entity.ToTable("LichSuNapTien");

            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayNap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SoTienThucNhan).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Hoàn thành");

            entity.HasOne(d => d.MaGoiNapNavigation).WithMany(p => p.LichSuNapTiens)
                .HasForeignKey(d => d.MaGoiNap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuNap__MaGoi__09A971A2");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.LichSuNapTienMaKhachHangNavigations)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuNap__MaKha__07C12930");

            entity.HasOne(d => d.TenDangNhapNavigation).WithMany(p => p.LichSuNapTienTenDangNhapNavigations)
                .HasPrincipalKey(p => p.TenDangNhap)
                .HasForeignKey(d => d.TenDangNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuNap__TenDa__08B54D69");
        });

        modelBuilder.Entity<MayTinh>(entity =>
        {
            entity.HasKey(e => e.MaMayTinh).HasName("PK__MayTinh__AC328530195A6F7B");

            entity.ToTable("MayTinh");

            entity.HasIndex(e => e.Stt, "UQ__MayTinh__CA1EB691BB71EB0F").IsUnique();

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Stt)
                .HasMaxLength(10)
                .HasColumnName("STT");
            entity.Property(e => e.TenMayTinh).HasMaxLength(100);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Tắt");

            entity.HasOne(d => d.MaKhuVucNavigation).WithMany(p => p.MayTinhs)
                .HasForeignKey(d => d.MaKhuVuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MayTinh__MaKhuVu__5AEE82B9");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D76273E45369");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__NguoiDun__55F68FC04315D23C").IsUnique();

            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.TenVaiTro).HasMaxLength(50);

            entity.HasOne(d => d.TenVaiTroNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.TenVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NguoiDung__TenVa__3B75D760");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NhanVien__77B2CA47F14A3097");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.SoDienThoai, "UQ__NhanVien__0389B7BD44C36243").IsUnique();

            entity.Property(e => e.CaLamViec).HasMaxLength(50);
            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.Luong).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanVien__MaNguo__4D94879B");
        });

        modelBuilder.Entity<NhiemVu>(entity =>
        {
            entity.HasKey(e => e.MaNhiemVu).HasName("PK__NhiemVu__69582B2FFB27284B");

            entity.ToTable("NhiemVu");

            entity.HasIndex(e => e.TenNhiemVu, "UQ__NhiemVu__9226A6C0918417D0").IsUnique();

            entity.Property(e => e.DieuKien).HasMaxLength(255);
            entity.Property(e => e.DoKho).HasDefaultValue(1);
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenNhiemVu).HasMaxLength(100);
        });

        modelBuilder.Entity<PhanThuongDaNhan>(entity =>
        {
            entity.HasKey(e => e.MaPhanThuong).HasName("PK__PhanThuo__41045ED76F8CFEC0");

            entity.ToTable("PhanThuongDaNhan");

            entity.Property(e => e.NgayNhan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.PhanThuongDaNhans)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhanThuon__MaKha__282DF8C2");

            entity.HasOne(d => d.MaPhuThuongNavigation).WithMany(p => p.PhanThuongDaNhans)
                .HasForeignKey(d => d.MaPhuThuong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhanThuon__MaPhu__29221CFB");
        });

        modelBuilder.Entity<PhienChoi>(entity =>
        {
            entity.HasKey(e => e.MaPhienChoi).HasName("PK__PhienCho__A91E798EB644A7F3");

            entity.ToTable("PhienChoi");

            entity.Property(e => e.ThoiGianBatDau)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime");
            entity.Property(e => e.ThongTinViTri).HasMaxLength(100);
            entity.Property(e => e.TongChiPhi)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang chơi");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.PhienChois)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhienChoi__MaKha__114A936A");

            entity.HasOne(d => d.MaMayTinhNavigation).WithMany(p => p.PhienChois)
                .HasForeignKey(d => d.MaMayTinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhienChoi__MaMay__123EB7A3");
        });

        modelBuilder.Entity<PhuThuong>(entity =>
        {
            entity.HasKey(e => e.MaPhuThuong).HasName("PK__PhuThuon__FE91359629C3B5C4");

            entity.ToTable("PhuThuong");

            entity.HasIndex(e => e.TenPhuThuong, "UQ__PhuThuon__E60F5B74295BC723").IsUnique();

            entity.Property(e => e.GiaTri).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LoaiPhuThuong).HasMaxLength(50);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenPhuThuong).HasMaxLength(100);

            entity.HasOne(d => d.MaVatPhamNavigation).WithMany(p => p.PhuThuongs)
                .HasForeignKey(d => d.MaVatPham)
                .HasConstraintName("FK__PhuThuong__MaVat__72C60C4A");
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.MaThongBao).HasName("PK__ThongBao__04DEB54EC3916779");

            entity.ToTable("ThongBao");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(255);
            entity.Property(e => e.ThongTinViTri).HasMaxLength(100);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang xử lý");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__ThongBao__MaKhac__367C1819");

            entity.HasOne(d => d.MaMayTinhNavigation).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.MaMayTinh)
                .HasConstraintName("FK__ThongBao__MaMayT__3864608B");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__ThongBao__MaNhan__37703C52");
        });

        modelBuilder.Entity<ThongKe>(entity =>
        {
            entity.HasKey(e => e.MaThongKe).HasName("PK__ThongKe__60E521F4D313D547");

            entity.ToTable("ThongKe");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongDoanhThu)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(15, 2)");
            entity.Property(e => e.TongDoanhThuDonHang)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(15, 2)");
            entity.Property(e => e.TongDoanhThuPhien)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(15, 2)");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.TenVaiTro).HasName("PK__VaiTro__1DA558156C230526");

            entity.ToTable("VaiTro");

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        modelBuilder.Entity<VatPham>(entity =>
        {
            entity.HasKey(e => e.MaVatPham).HasName("PK__VatPham__92DB0A984377EA4F");

            entity.ToTable("VatPham");

            entity.HasIndex(e => e.TenVatPham, "UQ__VatPham__277D36A01AA7A5F8").IsUnique();

            entity.Property(e => e.GiaMua).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenVatPham).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
