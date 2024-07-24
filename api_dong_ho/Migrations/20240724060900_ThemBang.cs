using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_dong_ho.Migrations
{
    /// <inheritdoc />
    public partial class ThemBang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loais",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loais", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "NhanHieus",
                columns: table => new
                {
                    MaNhanHieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhanHieu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanHieus", x => x.MaNhanHieu);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaLoai",
                table: "SanPham",
                column: "MaLoai");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_MaNhanHieu",
                table: "SanPham",
                column: "MaNhanHieu");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_Loais_MaLoai",
                table: "SanPham",
                column: "MaLoai",
                principalTable: "Loais",
                principalColumn: "MaLoai",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_NhanHieus_MaNhanHieu",
                table: "SanPham",
                column: "MaNhanHieu",
                principalTable: "NhanHieus",
                principalColumn: "MaNhanHieu",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_Loais_MaLoai",
                table: "SanPham");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_NhanHieus_MaNhanHieu",
                table: "SanPham");

            migrationBuilder.DropTable(
                name: "Loais");

            migrationBuilder.DropTable(
                name: "NhanHieus");

            migrationBuilder.DropIndex(
                name: "IX_SanPham_MaLoai",
                table: "SanPham");

            migrationBuilder.DropIndex(
                name: "IX_SanPham_MaNhanHieu",
                table: "SanPham");
        }
    }
}
