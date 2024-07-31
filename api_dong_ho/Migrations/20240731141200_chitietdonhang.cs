using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_dong_ho.Migrations
{
    /// <inheritdoc />
    public partial class chitietdonhang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_DonHangs_MaDH",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_KichThuoc_MaKichThuoc",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_MauSac_MaMauSac",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_SanPham_MaSP",
                table: "ChiTietDonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChiTietDonHang",
                table: "ChiTietDonHang");

            migrationBuilder.RenameTable(
                name: "ChiTietDonHang",
                newName: "chiTietDonHangs");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDonHang_MaSP",
                table: "chiTietDonHangs",
                newName: "IX_chiTietDonHangs_MaSP");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDonHang_MaMauSac",
                table: "chiTietDonHangs",
                newName: "IX_chiTietDonHangs_MaMauSac");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDonHang_MaKichThuoc",
                table: "chiTietDonHangs",
                newName: "IX_chiTietDonHangs_MaKichThuoc");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietDonHang_MaDH",
                table: "chiTietDonHangs",
                newName: "IX_chiTietDonHangs_MaDH");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chiTietDonHangs",
                table: "chiTietDonHangs",
                column: "MaCTDH");

            migrationBuilder.AddForeignKey(
                name: "FK_chiTietDonHangs_DonHangs_MaDH",
                table: "chiTietDonHangs",
                column: "MaDH",
                principalTable: "DonHangs",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK_chiTietDonHangs_KichThuoc_MaKichThuoc",
                table: "chiTietDonHangs",
                column: "MaKichThuoc",
                principalTable: "KichThuoc",
                principalColumn: "MaKichThuoc",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chiTietDonHangs_MauSac_MaMauSac",
                table: "chiTietDonHangs",
                column: "MaMauSac",
                principalTable: "MauSac",
                principalColumn: "MaMauSac",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_chiTietDonHangs_SanPham_MaSP",
                table: "chiTietDonHangs",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chiTietDonHangs_DonHangs_MaDH",
                table: "chiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_chiTietDonHangs_KichThuoc_MaKichThuoc",
                table: "chiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_chiTietDonHangs_MauSac_MaMauSac",
                table: "chiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_chiTietDonHangs_SanPham_MaSP",
                table: "chiTietDonHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chiTietDonHangs",
                table: "chiTietDonHangs");

            migrationBuilder.RenameTable(
                name: "chiTietDonHangs",
                newName: "ChiTietDonHang");

            migrationBuilder.RenameIndex(
                name: "IX_chiTietDonHangs_MaSP",
                table: "ChiTietDonHang",
                newName: "IX_ChiTietDonHang_MaSP");

            migrationBuilder.RenameIndex(
                name: "IX_chiTietDonHangs_MaMauSac",
                table: "ChiTietDonHang",
                newName: "IX_ChiTietDonHang_MaMauSac");

            migrationBuilder.RenameIndex(
                name: "IX_chiTietDonHangs_MaKichThuoc",
                table: "ChiTietDonHang",
                newName: "IX_ChiTietDonHang_MaKichThuoc");

            migrationBuilder.RenameIndex(
                name: "IX_chiTietDonHangs_MaDH",
                table: "ChiTietDonHang",
                newName: "IX_ChiTietDonHang_MaDH");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChiTietDonHang",
                table: "ChiTietDonHang",
                column: "MaCTDH");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_DonHangs_MaDH",
                table: "ChiTietDonHang",
                column: "MaDH",
                principalTable: "DonHangs",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_KichThuoc_MaKichThuoc",
                table: "ChiTietDonHang",
                column: "MaKichThuoc",
                principalTable: "KichThuoc",
                principalColumn: "MaKichThuoc",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_MauSac_MaMauSac",
                table: "ChiTietDonHang",
                column: "MaMauSac",
                principalTable: "MauSac",
                principalColumn: "MaMauSac",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_SanPham_MaSP",
                table: "ChiTietDonHang",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");
        }
    }
}
