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

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-DOHM20E;Database=QuanLyNet;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DichVu>(entity =>
        {
            entity.HasKey(e => e.MaDichVu).HasName("PK__DichVu__C0E6DE8FB7D4ECFE");

            entity.ToTable("DichVu");

            entity.Property(e => e.Gia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LoaiDichVu).HasMaxLength(50);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDichVu).HasMaxLength(100);
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonHang__129584ADC941F665");

            entity.ToTable("DonHang");

            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang xử lý");

            entity.HasOne(d => d.MaDichVuNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaDichVu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaDichV__5AEE82B9");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaKhach__59FA5E80");

            entity.HasOne(d => d.MaKhuyenMaiNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKhuyenMai)
                .HasConstraintName("FK__DonHang__MaKhuye__5BE2A6F2");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E5E3B3B491");

            entity.ToTable("KhachHang");

            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDu)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<KhuVuc>(entity =>
        {
            entity.HasKey(e => e.MaKhuVuc).HasName("PK__KhuVuc__0676EB8375C8EF47");

            entity.ToTable("KhuVuc");

            entity.Property(e => e.HinhAnh)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenKhuVuc).HasMaxLength(100);
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKhuyenMai).HasName("PK__KhuyenMa__6F56B3BDA3B253EB");

            entity.ToTable("KhuyenMai");

            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayKetThuc).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhanTramGiamGia)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TenKhuyenMai).HasMaxLength(100);
        });

        modelBuilder.Entity<LichSuDangNhap>(entity =>
        {
            entity.HasKey(e => e.MaDangNhap).HasName("PK__LichSuDa__C869B8C0124516AC");

            entity.ToTable("LichSuDangNhap");

            entity.Property(e => e.ThoiGianDangNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.LichSuDangNhaps)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuDan__MaNgu__403A8C7D");
        });

        modelBuilder.Entity<LichSuNapTien>(entity =>
        {
            entity.HasKey(e => e.MaNapTien).HasName("PK__LichSuNa__86747C7688248168");

            entity.ToTable("LichSuNapTien");

            entity.Property(e => e.NgayNap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoTien).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.LichSuNapTiens)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuNap__MaKha__5FB337D6");
        });

        modelBuilder.Entity<MayTinh>(entity =>
        {
            entity.HasKey(e => e.MaMayTinh).HasName("PK__MayTinh__AC328530212344A7");

            entity.ToTable("MayTinh");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenMayTinh).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasDefaultValue(false);

            entity.HasOne(d => d.MaKhuVucNavigation).WithMany(p => p.MayTinhs)
                .HasForeignKey(d => d.MaKhuVuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MayTinh__MaKhuVu__4E88ABD4");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D762205E3672");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__NguoiDun__55F68FC0F8BF313E").IsUnique();

            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NguoiDung__MaVai__3C69FB99");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NhanVien__77B2CA4718C90C22");

            entity.ToTable("NhanVien");

            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__NhanVien__MaNguo__47DBAE45");
        });

        modelBuilder.Entity<NhiemVu>(entity =>
        {
            entity.HasKey(e => e.MaNhiemVu).HasName("PK__NhiemVu__69582B2F1AB9926C");

            entity.ToTable("NhiemVu");

            entity.Property(e => e.DoKho).HasDefaultValue(1);
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenNhiemVu).HasMaxLength(100);

            entity.HasOne(d => d.MaPhuThuongNavigation).WithMany(p => p.NhiemVus)
                .HasForeignKey(d => d.MaPhuThuong)
                .HasConstraintName("FK__NhiemVu__MaPhuTh__6D0D32F4");
        });

        modelBuilder.Entity<PhanThuongDaNhan>(entity =>
        {
            entity.HasKey(e => e.MaPhanThuong).HasName("PK__PhanThuo__41045ED73EF21FB1");

            entity.ToTable("PhanThuongDaNhan");

            entity.Property(e => e.NgayNhan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.PhanThuongDaNhans)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhanThuon__MaKha__70DDC3D8");

            entity.HasOne(d => d.MaPhuThuongNavigation).WithMany(p => p.PhanThuongDaNhans)
                .HasForeignKey(d => d.MaPhuThuong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhanThuon__MaPhu__71D1E811");
        });

        modelBuilder.Entity<PhienChoi>(entity =>
        {
            entity.HasKey(e => e.MaPhienChoi).HasName("PK__PhienCho__A91E798E437F5E94");

            entity.ToTable("PhienChoi");

            entity.Property(e => e.ThoiGianBatDau)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime");
            entity.Property(e => e.TongChiPhi)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.PhienChois)
                .HasForeignKey(d => d.MaKhachHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhienChoi__MaKha__6477ECF3");

            entity.HasOne(d => d.MaMayTinhNavigation).WithMany(p => p.PhienChois)
                .HasForeignKey(d => d.MaMayTinh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PhienChoi__MaMay__656C112C");
        });

        modelBuilder.Entity<PhuThuong>(entity =>
        {
            entity.HasKey(e => e.MaPhuThuong).HasName("PK__PhuThuon__FE913596AB24B19A");

            entity.ToTable("PhuThuong");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenPhuThuong).HasMaxLength(100);
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.MaThongBao).HasName("PK__ThongBao__04DEB54EC3D605AA");

            entity.ToTable("ThongBao");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(255);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Đang xử lý");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__ThongBao__MaKhac__797309D9");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__ThongBao__MaNhan__7A672E12");
        });

        modelBuilder.Entity<ThongKe>(entity =>
        {
            entity.HasKey(e => e.MaThongKe).HasName("PK__ThongKe__60E521F42F139541");

            entity.ToTable("ThongKe");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(15, 2)");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VaiTro__C24C41CF92F914C7");

            entity.ToTable("VaiTro");

            entity.HasIndex(e => e.TenVaiTro, "UQ__VaiTro__1DA55814412C66E9").IsUnique();

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        modelBuilder.Entity<VatPham>(entity =>
        {
            entity.HasKey(e => e.MaVatPham).HasName("PK__VatPham__92DB0A98B8396A5C");

            entity.ToTable("VatPham");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenVatPham).HasMaxLength(100);
        });
    }
}





