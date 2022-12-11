﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using _1.DAL.Models;

#nullable disable

namespace _1.DAL.Migrations
{
    [DbContext(typeof(PetShopDbContext))]
    partial class PetShopDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("_1.DAL.Models.ChucVu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ChucVu");
                });

            modelBuilder.Entity("_1.DAL.Models.CuaHang", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Sdt")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("CuaHang");
                });

            modelBuilder.Entity("_1.DAL.Models.DoChoi", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("DoChoi");
                });

            modelBuilder.Entity("_1.DAL.Models.DoChoiChiTiet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GiaBan")
                        .HasColumnType("int")
                        .HasColumnName("GiaBan");

                    b.Property<int>("GiaNhap")
                        .HasColumnType("int")
                        .HasColumnName("GiaNhap");

                    b.Property<Guid>("IdDoChoi")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Loai")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Loai");

                    b.Property<string>("Nsx")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Nsx");

                    b.Property<int>("SoLuongTon")
                        .HasColumnType("int")
                        .HasColumnName("SoLuongTon");

                    b.HasKey("Id");

                    b.HasIndex("IdDoChoi");

                    b.ToTable("DoChoiChiTiet", (string)null);
                });

            modelBuilder.Entity("_1.DAL.Models.GiongLoai", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("XuatXu")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("GiongLoai");
                });

            modelBuilder.Entity("_1.DAL.Models.HoaDon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("DiaChi");

                    b.Property<Guid>("IdKhachHang")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdKhachHang");

                    b.Property<Guid>("IdNhanVien")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdNhanVien");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasColumnName("Ma");

                    b.Property<DateTime>("NgayGiaoHang")
                        .HasColumnType("Datetime")
                        .HasColumnName("NgayGiaoHang");

                    b.Property<DateTime>("NgayNhan")
                        .HasColumnType("Datetime")
                        .HasColumnName("NgayNhan");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("Datetime")
                        .HasColumnName("NgayTao");

                    b.Property<DateTime>("NgayThanhToan")
                        .HasColumnType("Datetime")
                        .HasColumnName("NgayThanhToan");

                    b.Property<decimal>("PhanTramGiamGia")
                        .HasColumnType("decimal");

                    b.Property<string>("Sdt")
                        .IsRequired()
                        .HasColumnType("varchar(11)")
                        .HasColumnName("Sdt");

                    b.Property<string>("TenNguoiNhan")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("TenNguoiNhan");

                    b.Property<decimal>("TienCoc")
                        .HasColumnType("decimal");

                    b.Property<decimal>("TienShip")
                        .HasColumnType("decimal");

                    b.Property<int>("TinhTrang")
                        .HasColumnType("int")
                        .HasColumnName("TinhTrang");

                    b.HasKey("Id");

                    b.HasIndex("IdKhachHang");

                    b.HasIndex("IdNhanVien");

                    b.ToTable("HoaDon");
                });

            modelBuilder.Entity("_1.DAL.Models.HoaDonChiTiet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DonGia")
                        .HasColumnType("Decimal")
                        .HasColumnName("DonGia");

                    b.Property<Guid>("IdHoaDon")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdThuCungChiTiet")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IdThuCungCT");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int")
                        .HasColumnName("SoLuong");

                    b.HasKey("Id");

                    b.HasIndex("IdHoaDon");

                    b.HasIndex("IdThuCungChiTiet");

                    b.ToTable("HoaDonChiTiet");
                });

            modelBuilder.Entity("_1.DAL.Models.HoaDonDoChoiChiTiet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DonGia")
                        .HasColumnType("Decimal")
                        .HasColumnName("DonGia");

                    b.Property<Guid>("IdDoChoiChiTiet")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdHoaDon")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int")
                        .HasColumnName("SoLuong");

                    b.HasKey("Id");

                    b.HasIndex("IdDoChoiChiTiet");

                    b.HasIndex("IdHoaDon");

                    b.ToTable("HoaDonDoChoiChiTiet");
                });

            modelBuilder.Entity("_1.DAL.Models.HoaDonThucAnChiTiet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DonGia")
                        .HasColumnType("Decimal")
                        .HasColumnName("DonGia");

                    b.Property<Guid>("IdHoaDon")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdThucAnChiTiet")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int")
                        .HasColumnName("SoLuong");

                    b.HasKey("Id");

                    b.HasIndex("IdHoaDon");

                    b.HasIndex("IdThucAnChiTiet");

                    b.ToTable("HoaDonThucAnChiTiet");
                });

            modelBuilder.Entity("_1.DAL.Models.KhachHang", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("DiaChi");

                    b.Property<string>("GioiTinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("GioiTinh");

                    b.Property<string>("Ho")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("HoKH");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("MaKH");

                    b.Property<DateTime>("NgaySinh")
                        .HasColumnType("Datetime")
                        .HasColumnName("NgaySinh");

                    b.Property<string>("QuocGia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("QuocGia");

                    b.Property<string>("Sdt")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("SĐT");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("TenKH");

                    b.Property<string>("TenDem")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("TenDemKH");

                    b.Property<string>("ThanhPho")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("ThanhPho");

                    b.HasKey("Id");

                    b.ToTable("KhachHang");
                });

            modelBuilder.Entity("_1.DAL.Models.MauSac", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("MauSac");
                });

            modelBuilder.Entity("_1.DAL.Models.NhanVien", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("GioiTinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Ho")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("IdChucVu")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdCuaHang")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("NgaySinh")
                        .HasColumnType("DateTime");

                    b.Property<string>("QuocGia")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Sdt")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TenDem")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ThanhPho")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdChucVu");

                    b.HasIndex("IdCuaHang");

                    b.ToTable("NhanVien");
                });

            modelBuilder.Entity("_1.DAL.Models.ThucAn", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ThucAn");
                });

            modelBuilder.Entity("_1.DAL.Models.ThucAnChiTiet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GiaBan")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("GiaNhap")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("IdThucAn")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Loai")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nsx")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SoLuongTon")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("IdThucAn");

                    b.ToTable("ThucAnChiTiet");
                });

            modelBuilder.Entity("_1.DAL.Models.ThuCung", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ThuCung");
                });

            modelBuilder.Entity("_1.DAL.Models.ThuCungChiTiet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("CanNang")
                        .HasColumnType("decimal");

                    b.Property<decimal>("ChieuDai")
                        .HasColumnType("decimal");

                    b.Property<decimal>("GiaBan")
                        .HasColumnType("decimal");

                    b.Property<decimal>("GiaNhap")
                        .HasColumnType("decimal");

                    b.Property<string>("GioiTinh")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid>("IdGiongLoai")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdMauSac")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdThuCung")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<int>("TrangThai")
                        .HasColumnType("int");

                    b.Property<int>("Tuoi")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdGiongLoai");

                    b.HasIndex("IdMauSac");

                    b.HasIndex("IdThuCung");

                    b.ToTable("ThuCungChiTiet");
                });

            modelBuilder.Entity("_1.DAL.Models.DoChoiChiTiet", b =>
                {
                    b.HasOne("_1.DAL.Models.DoChoi", "DoChoi")
                        .WithMany()
                        .HasForeignKey("IdDoChoi")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoChoi");
                });

            modelBuilder.Entity("_1.DAL.Models.HoaDon", b =>
                {
                    b.HasOne("_1.DAL.Models.KhachHang", "KhachHang")
                        .WithMany()
                        .HasForeignKey("IdKhachHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_1.DAL.Models.NhanVien", "NhanVien")
                        .WithMany()
                        .HasForeignKey("IdNhanVien")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");

                    b.Navigation("NhanVien");
                });

            modelBuilder.Entity("_1.DAL.Models.HoaDonChiTiet", b =>
                {
                    b.HasOne("_1.DAL.Models.HoaDon", "HoaDon")
                        .WithMany()
                        .HasForeignKey("IdHoaDon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_1.DAL.Models.ThuCungChiTiet", "ThuCungChiTiet")
                        .WithMany()
                        .HasForeignKey("IdThuCungChiTiet")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HoaDon");

                    b.Navigation("ThuCungChiTiet");
                });

            modelBuilder.Entity("_1.DAL.Models.HoaDonDoChoiChiTiet", b =>
                {
                    b.HasOne("_1.DAL.Models.DoChoiChiTiet", "DoChoiChiTiet")
                        .WithMany()
                        .HasForeignKey("IdDoChoiChiTiet")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_1.DAL.Models.HoaDon", "HoaDon")
                        .WithMany()
                        .HasForeignKey("IdHoaDon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoChoiChiTiet");

                    b.Navigation("HoaDon");
                });

            modelBuilder.Entity("_1.DAL.Models.HoaDonThucAnChiTiet", b =>
                {
                    b.HasOne("_1.DAL.Models.HoaDon", "HoaDon")
                        .WithMany()
                        .HasForeignKey("IdHoaDon")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_1.DAL.Models.ThucAnChiTiet", "ThucAnChiTiet")
                        .WithMany()
                        .HasForeignKey("IdThucAnChiTiet")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HoaDon");

                    b.Navigation("ThucAnChiTiet");
                });

            modelBuilder.Entity("_1.DAL.Models.NhanVien", b =>
                {
                    b.HasOne("_1.DAL.Models.ChucVu", "ChucVu")
                        .WithMany()
                        .HasForeignKey("IdChucVu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_1.DAL.Models.CuaHang", "CuaHang")
                        .WithMany()
                        .HasForeignKey("IdCuaHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChucVu");

                    b.Navigation("CuaHang");
                });

            modelBuilder.Entity("_1.DAL.Models.ThucAnChiTiet", b =>
                {
                    b.HasOne("_1.DAL.Models.ThucAn", "ThucAn")
                        .WithMany()
                        .HasForeignKey("IdThucAn")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ThucAn");
                });

            modelBuilder.Entity("_1.DAL.Models.ThuCungChiTiet", b =>
                {
                    b.HasOne("_1.DAL.Models.GiongLoai", "GiongLoai")
                        .WithMany()
                        .HasForeignKey("IdGiongLoai")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_1.DAL.Models.MauSac", "MauSac")
                        .WithMany()
                        .HasForeignKey("IdMauSac")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("_1.DAL.Models.ThuCung", "ThuCung")
                        .WithMany()
                        .HasForeignKey("IdThuCung")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GiongLoai");

                    b.Navigation("MauSac");

                    b.Navigation("ThuCung");
                });
#pragma warning restore 612, 618
        }
    }
}
