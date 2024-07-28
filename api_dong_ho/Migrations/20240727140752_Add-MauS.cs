using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_dong_ho.Migrations
{
    /// <inheritdoc />
    public partial class AddMauS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KichThuoc",
                columns: table => new
                {
                    MaKichThuoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKichThuoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaSP = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KichThuoc", x => x.MaKichThuoc);
                    table.ForeignKey(
                        name: "FK_KichThuoc_SanPham_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPham",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateTable(
                name: "MauSac",
                columns: table => new
                {
                    MaMauSac = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMauSac = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaSP = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauSac", x => x.MaMauSac);
                    table.ForeignKey(
                        name: "FK_MauSac_SanPham_MaSP",
                        column: x => x.MaSP,
                        principalTable: "SanPham",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KichThuoc_MaSP",
                table: "KichThuoc",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_MauSac_MaSP",
                table: "MauSac",
                column: "MaSP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KichThuoc");

            migrationBuilder.DropTable(
                name: "MauSac");
        }
    }
}
