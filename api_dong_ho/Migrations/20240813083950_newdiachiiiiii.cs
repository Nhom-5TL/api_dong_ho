using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_dong_ho.Migrations
{
    /// <inheritdoc />
    public partial class newdiachiiiiii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuanHuyen",
                table: "DonHangs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TinhThanh",
                table: "DonHangs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "XaPhuong",
                table: "DonHangs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuanHuyen",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "TinhThanh",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "XaPhuong",
                table: "DonHangs");
        }
    }
}
