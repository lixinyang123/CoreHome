using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreHome.Data.Migrations
{
    public partial class AddArchive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MonthId",
                table: "Articles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearId",
                table: "Articles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Years",
                columns: table => new
                {
                    Value = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Years", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "Months",
                columns: table => new
                {
                    Value = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    YearId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Months", x => x.Value);
                    table.ForeignKey(
                        name: "FK_Months_Years_YearId",
                        column: x => x.YearId,
                        principalTable: "Years",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_MonthId",
                table: "Articles",
                column: "MonthId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_YearId",
                table: "Articles",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_Months_YearId",
                table: "Months",
                column: "YearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Months_MonthId",
                table: "Articles",
                column: "MonthId",
                principalTable: "Months",
                principalColumn: "Value",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Years_YearId",
                table: "Articles",
                column: "YearId",
                principalTable: "Years",
                principalColumn: "Value",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Months_MonthId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Years_YearId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "Months");

            migrationBuilder.DropTable(
                name: "Years");

            migrationBuilder.DropIndex(
                name: "IX_Articles_MonthId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_YearId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "MonthId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "YearId",
                table: "Articles");
        }
    }
}
