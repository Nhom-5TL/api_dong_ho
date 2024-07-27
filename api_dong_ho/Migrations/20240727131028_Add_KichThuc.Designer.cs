﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api_dong_ho.Data;

#nullable disable

namespace api_dong_ho.Migrations
{
    [DbContext(typeof(api_dong_hoContext))]
    [Migration("20240727131028_Add_KichThuc")]
    partial class Add_KichThuc
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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
#pragma warning restore 612, 618
        }
    }
}
