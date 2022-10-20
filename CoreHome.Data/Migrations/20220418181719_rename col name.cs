using Microsoft.EntityFrameworkCore.Migrations;


namespace CoreHome.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Notifications",
                newName: "Time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.RenameColumn(
                name: "Time",
                table: "Notifications",
                newName: "DateTime");
        }
    }
}
