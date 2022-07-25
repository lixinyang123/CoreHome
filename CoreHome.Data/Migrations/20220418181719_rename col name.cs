using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreHome.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Notifications",
                newName: "Time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Notifications",
                newName: "DateTime");
        }
    }
}
