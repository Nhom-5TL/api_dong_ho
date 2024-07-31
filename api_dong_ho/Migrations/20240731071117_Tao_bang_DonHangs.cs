using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_dong_ho.Migrations
{
    /// <inheritdoc />
    public partial class Tao_bang_DonHangs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_DonHang_MaDH",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_KhachHang_MaKh",
                table: "DonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonHang",
                table: "DonHang");

            migrationBuilder.RenameTable(
                name: "DonHang",
                newName: "DonHangs");

            migrationBuilder.RenameIndex(
                name: "IX_DonHang_MaKh",
                table: "DonHangs",
                newName: "IX_DonHangs_MaKh");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonHangs",
                table: "DonHangs",
                column: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_DonHangs_MaDH",
                table: "ChiTietDonHang",
                column: "MaDH",
                principalTable: "DonHangs",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_KhachHang_MaKh",
                table: "DonHangs",
                column: "MaKh",
                principalTable: "KhachHang",
                principalColumn: "MaKH",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietDonHang_DonHangs_MaDH",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_KhachHang_MaKh",
                table: "DonHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonHangs",
                table: "DonHangs");

            migrationBuilder.RenameTable(
                name: "DonHangs",
                newName: "DonHang");

            migrationBuilder.RenameIndex(
                name: "IX_DonHangs_MaKh",
                table: "DonHang",
                newName: "IX_DonHang_MaKh");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonHang",
                table: "DonHang",
                column: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietDonHang_DonHang_MaDH",
                table: "ChiTietDonHang",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");
            
            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_KhachHang_MaKh",
                table: "DonHang",
                column: "MaKh",
                principalTable: "KhachHang",
                principalColumn: "MaKH",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
