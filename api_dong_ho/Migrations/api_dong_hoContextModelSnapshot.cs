﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api_dong_ho.Dtos;

#nullable disable

namespace api_dong_ho.Migrations
{
    [DbContext(typeof(api_dong_hoContext))]
    partial class api_dong_hoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("api_dong_ho.Models.ChiTietDonHang", b =>
                {
                    b.Property<int?>("MaCTDH")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("MaCTDH"));

                    b.Property<int?>("DonGia")
                        .HasColumnType("int");

                    b.Property<int?>("MaDH")
                        .HasColumnType("int");

                    b.Property<int>("MaKichThuoc")
                        .HasColumnType("int");

                    b.Property<int>("MaMauSac")
                        .HasColumnType("int");

                    b.Property<int?>("MaSP")
                        .HasColumnType("int");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<string>("TenSP")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaCTDH");

                    b.HasIndex("MaDH");

                    b.HasIndex("MaKichThuoc");

                    b.HasIndex("MaMauSac");

                    b.HasIndex("MaSP");

                    b.ToTable("ChiTietDonHang");
                });

            modelBuilder.Entity("api_dong_ho.Models.DonHang", b =>
                {
                    b.Property<int>("MaDH")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDH"));

                    b.Property<string>("DiaChi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GhiChu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaKh")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenKh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrangThai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaDH");

                    b.HasIndex("MaKh");

                    b.ToTable("DonHang");
                });

            modelBuilder.Entity("api_dong_ho.Models.KhachHang", b =>
                {
                    b.Property<int>("MaKH")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaKH"));

                    b.Property<string>("CCCD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("SDT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenKh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenTaiKhoan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrangThai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaKH");

                    b.ToTable("KhachHang");
                });

            modelBuilder.Entity("api_dong_ho.Models.KichThuoc", b =>
                {
                    b.Property<int>("MaKichThuoc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaKichThuoc"));

                    b.Property<int?>("MaSP")
                        .HasColumnType("int");

                    b.Property<string>("TenKichThuoc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaKichThuoc");

                    b.HasIndex("MaSP");

                    b.ToTable("KichThuoc");
                });

            modelBuilder.Entity("api_dong_ho.Models.Loai", b =>
                {
                    b.Property<int>("MaLoai")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaLoai"));

                    b.Property<string>("TenLoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaLoai");

                    b.ToTable("Loais");
                });

            modelBuilder.Entity("api_dong_ho.Models.MauSac", b =>
                {
                    b.Property<int>("MaMauSac")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaMauSac"));

                    b.Property<int?>("MaSP")
                        .HasColumnType("int");

                    b.Property<string>("TenMauSac")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaMauSac");

                    b.HasIndex("MaSP");

                    b.ToTable("MauSac");
                });

            modelBuilder.Entity("api_dong_ho.Models.NhanHieu", b =>
                {
                    b.Property<int>("MaNhanHieu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNhanHieu"));

                    b.Property<string>("TenNhanHieu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MaNhanHieu");

                    b.ToTable("NhanHieus");
                });

            modelBuilder.Entity("api_dong_ho.Models.SanPham", b =>
                {
                    b.Property<int>("MaSP")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaSP"));

                    b.Property<int>("Gia")
                        .HasColumnType("int");

                    b.Property<int>("HTVC")
                        .HasColumnType("int");

                    b.Property<string>("HinhAnh")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaLoai")
                        .HasColumnType("int");

                    b.Property<int>("MaNhanHieu")
                        .HasColumnType("int");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenSP")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<int>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("MaSP");

                    b.HasIndex("MaLoai");

                    b.HasIndex("MaNhanHieu");

                    b.ToTable("SanPham");
                });

            modelBuilder.Entity("api_dong_ho.Models.ChiTietDonHang", b =>
                {
                    b.HasOne("api_dong_ho.Models.DonHang", "DonHang")
                        .WithMany("ChiTietDonHangs")
                        .HasForeignKey("MaDH");

                    b.HasOne("api_dong_ho.Models.KichThuoc", "KichThuoc")
                        .WithMany("ChiTietDonHangs")
                        .HasForeignKey("MaKichThuoc")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api_dong_ho.Models.MauSac", "MauSac")
                        .WithMany("ChiTietDonHangs")
                        .HasForeignKey("MaMauSac")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api_dong_ho.Models.SanPham", "SanPham")
                        .WithMany("ChiTietDonHangs")
                        .HasForeignKey("MaSP");

                    b.Navigation("DonHang");

                    b.Navigation("KichThuoc");

                    b.Navigation("MauSac");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("api_dong_ho.Models.DonHang", b =>
                {
                    b.HasOne("api_dong_ho.Models.KhachHang", "KhachHang")
                        .WithMany("DonHangs")
                        .HasForeignKey("MaKh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KhachHang");
                });

            modelBuilder.Entity("api_dong_ho.Models.KichThuoc", b =>
                {
                    b.HasOne("api_dong_ho.Models.SanPham", "SanPham")
                        .WithMany("KichThuocs")
                        .HasForeignKey("MaSP");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("api_dong_ho.Models.MauSac", b =>
                {
                    b.HasOne("api_dong_ho.Models.SanPham", "SanPham")
                        .WithMany("MauSacs")
                        .HasForeignKey("MaSP");

                    b.Navigation("SanPham");
                });

            modelBuilder.Entity("api_dong_ho.Models.SanPham", b =>
                {
                    b.HasOne("api_dong_ho.Models.Loai", "Loais")
                        .WithMany()
                        .HasForeignKey("MaLoai")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("api_dong_ho.Models.NhanHieu", "NhanHieus")
                        .WithMany()
                        .HasForeignKey("MaNhanHieu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Loais");

                    b.Navigation("NhanHieus");
                });

            modelBuilder.Entity("api_dong_ho.Models.DonHang", b =>
                {
                    b.Navigation("ChiTietDonHangs");
                });

            modelBuilder.Entity("api_dong_ho.Models.KhachHang", b =>
                {
                    b.Navigation("DonHangs");
                });

            modelBuilder.Entity("api_dong_ho.Models.KichThuoc", b =>
                {
                    b.Navigation("ChiTietDonHangs");
                });

            modelBuilder.Entity("api_dong_ho.Models.MauSac", b =>
                {
                    b.Navigation("ChiTietDonHangs");
                });

            modelBuilder.Entity("api_dong_ho.Models.SanPham", b =>
                {
                    b.Navigation("ChiTietDonHangs");

                    b.Navigation("KichThuocs");

                    b.Navigation("MauSacs");
                });
#pragma warning restore 612, 618
        }
    }
}
