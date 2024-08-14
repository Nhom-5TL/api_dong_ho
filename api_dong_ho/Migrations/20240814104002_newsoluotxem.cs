using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_dong_ho.Migrations
{
    /// <inheritdoc />
    public partial class newsoluotxem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoLuotXem",
                table: "SanPham",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoLuotXem",
                table: "SanPham");
        }
    }
}
