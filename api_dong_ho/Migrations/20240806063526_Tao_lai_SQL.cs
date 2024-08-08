using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_dong_ho.Migrations
{
    /// <inheritdoc />
    public partial class Tao_lai_SQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KichThuoc_SanPham_MaSP",
                table: "KichThuoc");

            migrationBuilder.DropForeignKey(
                name: "FK_MauSac_SanPham_MaSP",
                table: "MauSac");

            migrationBuilder.AlterColumn<string>(
                name: "TenMauSac",
                table: "MauSac",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "MaSP",
                table: "MauSac",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenKichThuoc",
                table: "KichThuoc",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "MaSP",
                table: "KichThuoc",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenHinhAnh",
                table: "HinhAnhs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_KichThuoc_SanPham_MaSP",
                table: "KichThuoc",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MauSac_SanPham_MaSP",
                table: "MauSac",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KichThuoc_SanPham_MaSP",
                table: "KichThuoc");

            migrationBuilder.DropForeignKey(
                name: "FK_MauSac_SanPham_MaSP",
                table: "MauSac");

            migrationBuilder.AlterColumn<string>(
                name: "TenMauSac",
                table: "MauSac",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaSP",
                table: "MauSac",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TenKichThuoc",
                table: "KichThuoc",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaSP",
                table: "KichThuoc",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TenHinhAnh",
                table: "HinhAnhs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KichThuoc_SanPham_MaSP",
                table: "KichThuoc",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK_MauSac_SanPham_MaSP",
                table: "MauSac",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");
        }
    }
}
