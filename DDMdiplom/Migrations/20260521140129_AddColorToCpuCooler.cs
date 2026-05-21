using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDMdiplom.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToCpuCooler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "CpuCoolers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "CpuCoolers");
        }
    }
}
