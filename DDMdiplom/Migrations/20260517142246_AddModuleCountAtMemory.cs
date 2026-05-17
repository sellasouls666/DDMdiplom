using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDMdiplom.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleCountAtMemory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleCount",
                table: "Memory",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleCount",
                table: "Memory");
        }
    }
}
