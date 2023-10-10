using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoRedisCache.Migrations
{
    /// <inheritdoc />
    public partial class updateAllDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaLop",
                table: "SinhViens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "chuongTrinhs",
                columns: table => new
                {
                    MaCT = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenCT = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chuongTrinhs", x => x.MaCT);
                });

            migrationBuilder.CreateTable(
                name: "khoaHocs",
                columns: table => new
                {
                    MaKhoaHoc = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NamBatDau = table.Column<int>(type: "int", nullable: false),
                    NamKetThuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khoaHocs", x => x.MaKhoaHoc);
                });

            migrationBuilder.CreateTable(
                name: "khoas",
                columns: table => new
                {
                    MaKhoa = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenKhoa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NamThanhLap = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khoas", x => x.MaKhoa);
                });

            migrationBuilder.CreateTable(
                name: "Lops",
                columns: table => new
                {
                    MaLop = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKhoaHoc = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaKhoa = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaCT = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    soThuTu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lops", x => x.MaLop);
                    table.ForeignKey(
                        name: "FK_Lops_chuongTrinhs_MaCT",
                        column: x => x.MaCT,
                        principalTable: "chuongTrinhs",
                        principalColumn: "MaCT");
                    table.ForeignKey(
                        name: "FK_Lops_khoaHocs_MaKhoaHoc",
                        column: x => x.MaKhoaHoc,
                        principalTable: "khoaHocs",
                        principalColumn: "MaKhoaHoc");
                    table.ForeignKey(
                        name: "FK_Lops_khoas_MaKhoa",
                        column: x => x.MaKhoa,
                        principalTable: "khoas",
                        principalColumn: "MaKhoa");
                });

            migrationBuilder.CreateTable(
                name: "MonHocs",
                columns: table => new
                {
                    MaMH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tenMonHoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaKhoa = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonHocs", x => x.MaMH);
                    table.ForeignKey(
                        name: "FK_MonHocs_khoas_MaKhoa",
                        column: x => x.MaKhoa,
                        principalTable: "khoas",
                        principalColumn: "MaKhoa");
                });

            migrationBuilder.CreateTable(
                name: "KetQuas",
                columns: table => new
                {
                    MaSv = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaMH = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    lanThi = table.Column<int>(type: "int", nullable: false),
                    Diem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuas", x => x.MaSv);
                    table.ForeignKey(
                        name: "FK_KetQuas_MonHocs_MaMH",
                        column: x => x.MaMH,
                        principalTable: "MonHocs",
                        principalColumn: "MaMH");
                    table.ForeignKey(
                        name: "FK_KetQuas_khoas_MaSv",
                        column: x => x.MaSv,
                        principalTable: "khoas",
                        principalColumn: "MaKhoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaLop",
                table: "SinhViens",
                column: "MaLop");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuas_MaMH",
                table: "KetQuas",
                column: "MaMH");

            migrationBuilder.CreateIndex(
                name: "IX_Lops_MaCT",
                table: "Lops",
                column: "MaCT");

            migrationBuilder.CreateIndex(
                name: "IX_Lops_MaKhoa",
                table: "Lops",
                column: "MaKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_Lops_MaKhoaHoc",
                table: "Lops",
                column: "MaKhoaHoc");

            migrationBuilder.CreateIndex(
                name: "IX_MonHocs_MaKhoa",
                table: "MonHocs",
                column: "MaKhoa");

            migrationBuilder.AddForeignKey(
                name: "FK_SinhViens_Lops_MaLop",
                table: "SinhViens",
                column: "MaLop",
                principalTable: "Lops",
                principalColumn: "MaLop",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SinhViens_Lops_MaLop",
                table: "SinhViens");

            migrationBuilder.DropTable(
                name: "KetQuas");

            migrationBuilder.DropTable(
                name: "Lops");

            migrationBuilder.DropTable(
                name: "MonHocs");

            migrationBuilder.DropTable(
                name: "chuongTrinhs");

            migrationBuilder.DropTable(
                name: "khoaHocs");

            migrationBuilder.DropTable(
                name: "khoas");

            migrationBuilder.DropIndex(
                name: "IX_SinhViens_MaLop",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "MaLop",
                table: "SinhViens");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
