using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DDMdiplom.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    CaseMaterial = table.Column<string>(type: "text", nullable: true),
                    MotherboardCompatibility = table.Column<string>(type: "text", nullable: true),
                    SidePanel = table.Column<string>(type: "text", nullable: true),
                    Internal35Bays = table.Column<string>(type: "text", nullable: true),
                    Internal25Bays = table.Column<string>(type: "text", nullable: true),
                    ExpansionSlots = table.Column<int>(type: "integer", nullable: true),
                    VerticalGpuSupport = table.Column<string>(type: "text", nullable: true),
                    FrontPorts = table.Column<string>(type: "text", nullable: true),
                    FanOptions = table.Column<string>(type: "text", nullable: true),
                    PreInstalledFans = table.Column<string>(type: "text", nullable: true),
                    RadiatorOptions = table.Column<string>(type: "text", nullable: true),
                    MaxGpuLength = table.Column<int>(type: "integer", nullable: true),
                    MaxCpuCoolerHeight = table.Column<int>(type: "integer", nullable: true),
                    MaxPsuLength = table.Column<int>(type: "integer", nullable: true),
                    Dimensions = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<string>(type: "text", nullable: true),
                    PackageContent = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CpuCoolers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    FanSize = table.Column<string>(type: "text", nullable: true),
                    CpuSocketCompatibility = table.Column<string>(type: "text", nullable: true),
                    BearingType = table.Column<string>(type: "text", nullable: true),
                    Rpm = table.Column<string>(type: "text", nullable: true),
                    AirFlow = table.Column<string>(type: "text", nullable: true),
                    NoiseLevel = table.Column<string>(type: "text", nullable: true),
                    HeatsinkMaterial = table.Column<string>(type: "text", nullable: true),
                    FanMountingTypesToHeatsink = table.Column<string>(type: "text", nullable: true),
                    MaxCpuCoolerHeight = table.Column<int>(type: "integer", nullable: true),
                    FanDimensions = table.Column<string>(type: "text", nullable: true),
                    HeatsinkDimensions = table.Column<string>(type: "text", nullable: true),
                    Features = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuCoolers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GraphicsCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: false),
                    PartNumber = table.Column<string>(type: "text", nullable: true),
                    Interface = table.Column<string>(type: "text", nullable: true),
                    ChipsetManufacturer = table.Column<string>(type: "text", nullable: true),
                    GpuSeries = table.Column<string>(type: "text", nullable: true),
                    Gpu = table.Column<string>(type: "text", nullable: true),
                    BoostClock = table.Column<int>(type: "integer", nullable: true),
                    CudaCores = table.Column<int>(type: "integer", nullable: true),
                    EffectiveMemoryClock = table.Column<int>(type: "integer", nullable: true),
                    MemorySize = table.Column<int>(type: "integer", nullable: true),
                    MemoryInterface = table.Column<int>(type: "integer", nullable: true),
                    MemoryType = table.Column<string>(type: "text", nullable: true),
                    DirectX = table.Column<string>(type: "text", nullable: true),
                    OpenGL = table.Column<string>(type: "text", nullable: true),
                    MultiMonitorSupport = table.Column<int>(type: "integer", nullable: true),
                    HdmiPorts = table.Column<int>(type: "integer", nullable: true),
                    DisplayPorts = table.Column<int>(type: "integer", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    MaxResolution = table.Column<string>(type: "text", nullable: true),
                    SliSupport = table.Column<string>(type: "text", nullable: true),
                    VirtualRealityReady = table.Column<string>(type: "text", nullable: true),
                    Cooler = table.Column<string>(type: "text", nullable: true),
                    ThermalDesignPower = table.Column<int>(type: "integer", nullable: true),
                    RecommendedPsuWattage = table.Column<int>(type: "integer", nullable: true),
                    PowerConnector = table.Column<string>(type: "text", nullable: true),
                    HdcpReady = table.Column<string>(type: "text", nullable: true),
                    FormFactor = table.Column<string>(type: "text", nullable: true),
                    MaxGpuLength = table.Column<int>(type: "integer", nullable: true),
                    CardDimensions = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphicsCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keyboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    KeyboardInterface = table.Column<string>(type: "text", nullable: true),
                    ConnectionType = table.Column<string>(type: "text", nullable: true),
                    DesignStyle = table.Column<string>(type: "text", nullable: true),
                    Layout = table.Column<string>(type: "text", nullable: true),
                    KeyboardColor = table.Column<string>(type: "text", nullable: true),
                    IsMechanical = table.Column<bool>(type: "boolean", nullable: true),
                    KeySwitch = table.Column<string>(type: "text", nullable: true),
                    KeySwitchType = table.Column<string>(type: "text", nullable: true),
                    FunctionKeysCount = table.Column<int>(type: "integer", nullable: true),
                    MouseIncluded = table.Column<string>(type: "text", nullable: true),
                    MouseInterface = table.Column<string>(type: "text", nullable: true),
                    TrackingMethod = table.Column<string>(type: "text", nullable: true),
                    MouseButtons = table.Column<int>(type: "integer", nullable: true),
                    HandOrientation = table.Column<string>(type: "text", nullable: true),
                    Features = table.Column<string>(type: "text", nullable: true),
                    PowerSource = table.Column<string>(type: "text", nullable: true),
                    SystemRequirement = table.Column<string>(type: "text", nullable: true),
                    Dimensions = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyboards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Memory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Speed = table.Column<string>(type: "text", nullable: true),
                    CasLatency = table.Column<string>(type: "text", nullable: true),
                    Timing = table.Column<string>(type: "text", nullable: true),
                    Voltage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    MultiChannelKit = table.Column<string>(type: "text", nullable: true),
                    BiosProfiles = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    HeatSpreader = table.Column<string>(type: "text", nullable: true),
                    LedColor = table.Column<string>(type: "text", nullable: true),
                    Features = table.Column<string>(type: "text", nullable: true),
                    RecommendUse = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    ConnectionType = table.Column<string>(type: "text", nullable: true),
                    Interface = table.Column<string>(type: "text", nullable: true),
                    GripStyle = table.Column<string>(type: "text", nullable: true),
                    TrackingMethod = table.Column<string>(type: "text", nullable: true),
                    MaxDpi = table.Column<int>(type: "integer", nullable: true),
                    HandOrientation = table.Column<string>(type: "text", nullable: true),
                    ButtonsCount = table.Column<int>(type: "integer", nullable: true),
                    ScrollingCapability = table.Column<string>(type: "text", nullable: true),
                    AdjustableWeight = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Lighting = table.Column<string>(type: "text", nullable: true),
                    PowerSource = table.Column<string>(type: "text", nullable: true),
                    BatteryLife = table.Column<string>(type: "text", nullable: true),
                    WirelessRange = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<string>(type: "text", nullable: true),
                    SupportedOperatingSystems = table.Column<string>(type: "text", nullable: true),
                    SystemRequirement = table.Column<string>(type: "text", nullable: true),
                    Features = table.Column<string>(type: "text", nullable: true),
                    PackageContents = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Monitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    PartNumber = table.Column<string>(type: "text", nullable: true),
                    MonitorType = table.Column<string>(type: "text", nullable: true),
                    CabinetColor = table.Column<string>(type: "text", nullable: true),
                    ScreenSize = table.Column<string>(type: "text", nullable: true),
                    Panel = table.Column<string>(type: "text", nullable: true),
                    DisplayType = table.Column<string>(type: "text", nullable: true),
                    AdaptiveSyncTechnology = table.Column<string>(type: "text", nullable: true),
                    MaximumResolution = table.Column<string>(type: "text", nullable: true),
                    Resolution = table.Column<string>(type: "text", nullable: true),
                    ViewingAngle = table.Column<string>(type: "text", nullable: true),
                    AspectRatio = table.Column<string>(type: "text", nullable: true),
                    BrightnessSdr = table.Column<int>(type: "integer", nullable: true),
                    BrightnessHdr = table.Column<int>(type: "integer", nullable: true),
                    ContrastRatio = table.Column<string>(type: "text", nullable: true),
                    ResponseTime = table.Column<string>(type: "text", nullable: true),
                    ColorGamut = table.Column<string>(type: "text", nullable: true),
                    DisplayColors = table.Column<string>(type: "text", nullable: true),
                    MonitorPixelDensity = table.Column<string>(type: "text", nullable: true),
                    RefreshRate = table.Column<int>(type: "integer", nullable: true),
                    HdrStandard = table.Column<string>(type: "text", nullable: true),
                    CurvedSurfaceScreen = table.Column<string>(type: "text", nullable: true),
                    HdmiPorts = table.Column<string>(type: "text", nullable: true),
                    DisplayPort = table.Column<string>(type: "text", nullable: true),
                    VideoPorts = table.Column<string>(type: "text", nullable: true),
                    Headphone = table.Column<string>(type: "text", nullable: true),
                    PowerSupply = table.Column<string>(type: "text", nullable: true),
                    StandAdjustments = table.Column<string>(type: "text", nullable: true),
                    VesaCompatibility = table.Column<string>(type: "text", nullable: true),
                    Dimensions = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<string>(type: "text", nullable: true),
                    ThreeDReady = table.Column<string>(type: "text", nullable: true),
                    BuiltInSpeakers = table.Column<string>(type: "text", nullable: true),
                    BuiltInWebcam = table.Column<string>(type: "text", nullable: true),
                    HdcpSupport = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motherboards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    CpuSocketType = table.Column<string>(type: "text", nullable: true),
                    CpuType = table.Column<string>(type: "text", nullable: true),
                    Chipset = table.Column<string>(type: "text", nullable: true),
                    MemorySlots = table.Column<int>(type: "integer", nullable: true),
                    MemoryStandard = table.Column<string>(type: "text", nullable: true),
                    MaxMemorySize = table.Column<int>(type: "integer", nullable: true),
                    EccSupported = table.Column<string>(type: "text", nullable: true),
                    PciExpress50x16 = table.Column<string>(type: "text", nullable: true),
                    PciExpressX4 = table.Column<string>(type: "text", nullable: true),
                    PciExpressX1 = table.Column<string>(type: "text", nullable: true),
                    SataPorts = table.Column<string>(type: "text", nullable: true),
                    M2Slots = table.Column<string>(type: "text", nullable: true),
                    AudioChipset = table.Column<string>(type: "text", nullable: true),
                    AudioChannels = table.Column<string>(type: "text", nullable: true),
                    LanChipset = table.Column<string>(type: "text", nullable: true),
                    MaxLanSpeed = table.Column<string>(type: "text", nullable: true),
                    WirelessLan = table.Column<string>(type: "text", nullable: true),
                    Bluetooth = table.Column<string>(type: "text", nullable: true),
                    RearPanelPorts = table.Column<string>(type: "text", nullable: true),
                    InternalIoConnectors = table.Column<string>(type: "text", nullable: true),
                    FormFactor = table.Column<string>(type: "text", nullable: true),
                    LedLighting = table.Column<string>(type: "text", nullable: true),
                    Dimensions = table.Column<string>(type: "text", nullable: true),
                    PowerConnectors = table.Column<string>(type: "text", nullable: true),
                    BiosFeature = table.Column<string>(type: "text", nullable: true),
                    Software = table.Column<string>(type: "text", nullable: true),
                    Features = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motherboards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperatingSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: true),
                    OperatingSystems = table.Column<string>(type: "text", nullable: true),
                    BitVersion = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<string>(type: "text", nullable: true),
                    SystemRequirements = table.Column<string>(type: "text", nullable: true),
                    Packaging = table.Column<string>(type: "text", nullable: true),
                    Disclaimer = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerSupplies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: false),
                    PartNumber = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    MaximumPower = table.Column<int>(type: "integer", nullable: true),
                    Fans = table.Column<string>(type: "text", nullable: true),
                    MainConnector = table.Column<string>(type: "text", nullable: true),
                    MaxPsuLength = table.Column<int>(type: "integer", nullable: true),
                    Modular = table.Column<string>(type: "text", nullable: true),
                    EnergyEfficient = table.Column<string>(type: "text", nullable: true),
                    InputVoltage = table.Column<string>(type: "text", nullable: true),
                    InputFrequencyRange = table.Column<string>(type: "text", nullable: true),
                    InputCurrent = table.Column<string>(type: "text", nullable: true),
                    Connectors = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerSupplies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: false),
                    DeviceType = table.Column<string>(type: "text", nullable: true),
                    FormFactor = table.Column<string>(type: "text", nullable: true),
                    Capacity = table.Column<string>(type: "text", nullable: true),
                    Interface = table.Column<string>(type: "text", nullable: true),
                    Rpm = table.Column<int>(type: "integer", nullable: true),
                    Cache = table.Column<int>(type: "integer", nullable: true),
                    RecordingTechnology = table.Column<string>(type: "text", nullable: true),
                    Protocol = table.Column<string>(type: "text", nullable: true),
                    MaxSequentialRead = table.Column<int>(type: "integer", nullable: true),
                    MaxSequentialWrite = table.Column<int>(type: "integer", nullable: true),
                    HeatSink = table.Column<string>(type: "text", nullable: true),
                    Dimensions = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric(10,4)", nullable: true),
                    Features = table.Column<string>(type: "text", nullable: true),
                    Usage = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpsDevices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Series = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: false),
                    InternationalVersion = table.Column<string>(type: "text", nullable: true),
                    InputVoltageRange = table.Column<string>(type: "text", nullable: true),
                    InputFrequency = table.Column<string>(type: "text", nullable: true),
                    InputConnection = table.Column<string>(type: "text", nullable: true),
                    VaRating = table.Column<int>(type: "integer", nullable: true),
                    Watts = table.Column<int>(type: "integer", nullable: true),
                    OutputVoltage = table.Column<string>(type: "text", nullable: true),
                    OutputFrequency = table.Column<string>(type: "text", nullable: true),
                    Outlets = table.Column<int>(type: "integer", nullable: true),
                    OutletType = table.Column<string>(type: "text", nullable: true),
                    BatteryType = table.Column<string>(type: "text", nullable: true),
                    BatteryRunTimeHalfLoad = table.Column<string>(type: "text", nullable: true),
                    BatteryRunTimeFullLoad = table.Column<string>(type: "text", nullable: true),
                    BatteryRechargeTime = table.Column<int>(type: "integer", nullable: true),
                    BatteryReplaceable = table.Column<string>(type: "text", nullable: true),
                    Alarms = table.Column<string>(type: "text", nullable: true),
                    InterfacePort = table.Column<string>(type: "text", nullable: true),
                    ManagementSoftware = table.Column<string>(type: "text", nullable: true),
                    Approvals = table.Column<string>(type: "text", nullable: true),
                    DataLineProtection = table.Column<string>(type: "text", nullable: true),
                    SurgeEnergyRating = table.Column<int>(type: "integer", nullable: true),
                    Protection = table.Column<string>(type: "text", nullable: true),
                    Features = table.Column<string>(type: "text", nullable: true),
                    Dimensions = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpsDevices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaterCoolers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    BlockCompatibility = table.Column<string>(type: "text", nullable: true),
                    BlockCompatibilityAmd = table.Column<string>(type: "text", nullable: true),
                    BlockCompatibilityIntel = table.Column<string>(type: "text", nullable: true),
                    BlockDimensions = table.Column<string>(type: "text", nullable: true),
                    PumpSpeed = table.Column<string>(type: "text", nullable: true),
                    PumpNoise = table.Column<string>(type: "text", nullable: true),
                    RadiatorSize = table.Column<string>(type: "text", nullable: true),
                    RadiatorMaterial = table.Column<string>(type: "text", nullable: true),
                    FanCount = table.Column<int>(type: "integer", nullable: true),
                    FanSize = table.Column<string>(type: "text", nullable: true),
                    FanDimensions = table.Column<string>(type: "text", nullable: true),
                    BearingType = table.Column<string>(type: "text", nullable: true),
                    FanRpm = table.Column<string>(type: "text", nullable: true),
                    FanAirFlow = table.Column<string>(type: "text", nullable: true),
                    FanNoise = table.Column<string>(type: "text", nullable: true),
                    Pressure = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    LedColor = table.Column<string>(type: "text", nullable: true),
                    TubeDimensions = table.Column<string>(type: "text", nullable: true),
                    TubeMaterial = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterCoolers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "CpuCoolers");

            migrationBuilder.DropTable(
                name: "GraphicsCards");

            migrationBuilder.DropTable(
                name: "Keyboards");

            migrationBuilder.DropTable(
                name: "Memory");

            migrationBuilder.DropTable(
                name: "Mice");

            migrationBuilder.DropTable(
                name: "Monitors");

            migrationBuilder.DropTable(
                name: "Motherboards");

            migrationBuilder.DropTable(
                name: "OperatingSystems");

            migrationBuilder.DropTable(
                name: "PowerSupplies");

            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropTable(
                name: "UpsDevices");

            migrationBuilder.DropTable(
                name: "WaterCoolers");
        }
    }
}
