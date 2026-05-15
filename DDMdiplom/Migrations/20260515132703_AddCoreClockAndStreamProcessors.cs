using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDMdiplom.Migrations
{
    /// <inheritdoc />
    public partial class AddCoreClockAndStreamProcessors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoreClock",
                table: "GraphicsCards",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StreamProcessors",
                table: "GraphicsCards",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoreClock",
                table: "GraphicsCards");

            migrationBuilder.DropColumn(
                name: "StreamProcessors",
                table: "GraphicsCards");
        }
    }
}
