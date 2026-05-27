using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDMdiplom.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCartItemToBuildReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentsJson",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "BuildId",
                table: "CartItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_BuildId",
                table: "CartItems",
                column: "BuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Builds_BuildId",
                table: "CartItems",
                column: "BuildId",
                principalTable: "Builds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Builds_BuildId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_BuildId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "BuildId",
                table: "CartItems");

            migrationBuilder.AddColumn<string>(
                name: "ComponentsJson",
                table: "CartItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
