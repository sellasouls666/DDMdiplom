using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDMdiplom.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CartItems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentsJson",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Name",
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
    }
}
