using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDMdiplom.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "WaterCoolers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "UpsDevices",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Storages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Processors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PowerSupplies",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "OperatingSystems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Motherboards",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Monitors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Mice",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Memory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Keyboards",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "GraphicsCards",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CpuCoolers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Cases",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "WaterCoolers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "UpsDevices");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Processors");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PowerSupplies");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OperatingSystems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Monitors");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Mice");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Memory");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Keyboards");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "GraphicsCards");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CpuCoolers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Cases");
        }
    }
}
