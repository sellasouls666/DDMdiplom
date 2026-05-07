using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DDMdiplom.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Processors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    ProcessorsType = table.Column<string>(type: "text", nullable: true),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: true),
                    CpuSocketType = table.Column<string>(type: "text", nullable: true),
                    CoreName = table.Column<string>(type: "text", nullable: true),
                    Cores = table.Column<int>(type: "integer", nullable: true),
                    Threads = table.Column<int>(type: "integer", nullable: true),
                    OperatingFrequency = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    MaxTurboFrequency = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    L1Cache = table.Column<string>(type: "text", nullable: true),
                    L2Cache = table.Column<string>(type: "text", nullable: true),
                    L3Cache = table.Column<string>(type: "text", nullable: true),
                    ManufacturingTech = table.Column<string>(type: "text", nullable: true),
                    InstructionSet = table.Column<string>(type: "text", nullable: true),
                    MemoryTypes = table.Column<string>(type: "text", nullable: true),
                    MemoryChannel = table.Column<int>(type: "integer", nullable: true),
                    MaxMemorySize = table.Column<string>(type: "text", nullable: true),
                    IntegratedGraphics = table.Column<string>(type: "text", nullable: true),
                    GraphicsBaseFrequency = table.Column<int>(type: "integer", nullable: true),
                    PciExpressRevision = table.Column<string>(type: "text", nullable: true),
                    MaxPciExpressLanes = table.Column<string>(type: "text", nullable: true),
                    ThermalDesignPower = table.Column<int>(type: "integer", nullable: true),
                    CoolingDevice = table.Column<string>(type: "text", nullable: true),
                    CompatibleChipsets = table.Column<string>(type: "text", nullable: true),
                    SupportedOperatingSystems = table.Column<string>(type: "text", nullable: true),
                    AdvancedTechnologies = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processors", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processors");
        }
    }
}
