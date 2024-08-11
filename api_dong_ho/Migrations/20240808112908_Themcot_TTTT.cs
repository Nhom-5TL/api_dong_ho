using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_dong_ho.Migrations
{
    /// <inheritdoc />
    public partial class Themcot_TTTT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrangThaiTT",
                table: "DonHangs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThaiTT",
                table: "DonHangs");
        }
    }
}
