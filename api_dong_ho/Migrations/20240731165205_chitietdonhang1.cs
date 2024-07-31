using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_dong_ho.Migrations
{
    /// <inheritdoc />
    public partial class chitietdonhang1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chiTietDonHangs_KichThuoc_MaKichThuoc",
                table: "chiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_chiTietDonHangs_MauSac_MaMauSac",
                table: "chiTietDonHangs");

            migrationBuilder.AlterColumn<int>(
                name: "MaMauSac",
                table: "chiTietDonHangs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MaKichThuoc",
                table: "chiTietDonHangs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_chiTietDonHangs_KichThuoc_MaKichThuoc",
                table: "chiTietDonHangs",
                column: "MaKichThuoc",
                principalTable: "KichThuoc",
                principalColumn: "MaKichThuoc");

            migrationBuilder.AddForeignKey(
                name: "FK_chiTietDonHangs_MauSac_MaMauSac",
                table: "chiTietDonHangs",
                column: "MaMauSac",
                principalTable: "MauSac",
                principalColumn: "MaMauSac");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chiTietDonHangs_KichThuoc_MaKichThuoc",
                table: "chiTietDonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_chiTietDonHangs_MauSac_MaMauSac",
                table: "chiTietDonHangs");

            migrationBuilder.AlterColumn<int>(
                name: "MaMauSac",
                table: "chiTietDonHangs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaKichThuoc",
                table: "chiTietDonHangs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
        }
    }
}
