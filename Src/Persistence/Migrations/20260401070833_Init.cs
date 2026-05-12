using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Coreapi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Subdomain = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ABN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyType = table.Column<int>(type: "int", nullable: false),
                    StaffRange = table.Column<int>(type: "int", nullable: false),
                    ChatUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RocketChatToken = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    RocketChatId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApiKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "b54ca753980367b64aa13147ae6a9cd9a0c05943e14782ebb53f17ad9c54da5b"),
                    Balance = table.Column<decimal>(type: "money", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProvinceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvinceCenter = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuarterTariffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuarterTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    AllotmentRoundTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<long>(type: "bigint", nullable: false),
                    ErtFee = table.Column<long>(type: "bigint", nullable: false),
                    CountErt = table.Column<int>(type: "int", nullable: false),
                    TestAndDeliveryFee = table.Column<long>(type: "bigint", nullable: false),
                    CountTestAndDelivery = table.Column<int>(type: "int", nullable: false),
                    IsQuota = table.Column<bool>(type: "bit", nullable: false),
                    JulianAllotmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarAllotmentDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    PercentIncrease = table.Column<float>(type: "real", nullable: false),
                    ErtApprovedFee = table.Column<long>(type: "bigint", nullable: false),
                    AddDifDays = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterTariffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoutName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoutAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoutAddressEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Routes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BankTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GatewayTypeEnum = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcceptorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sha256OfPan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetrievalReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemTraceAuditNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskedPan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianLocalDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarLocalDateTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Confirm = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransactions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuildingTariffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuildingGroupTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Tariff = table.Column<long>(type: "bigint", nullable: false),
                    MinTariff = table.Column<long>(type: "bigint", nullable: false),
                    BuildingGroupParameterTypeEnum = table.Column<int>(type: "int", nullable: false),
                    JulianDateExecute = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarDateExecute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SolarYear = table.Column<int>(type: "int", nullable: false),
                    Factor = table.Column<float>(type: "real", nullable: false),
                    TestAndDeliveryFactor = table.Column<float>(type: "real", nullable: false),
                    SupervisionTariff = table.Column<long>(type: "bigint", nullable: false),
                    SupervisionMinTariff = table.Column<long>(type: "bigint", nullable: false),
                    SupervisionFactor = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingTariffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingTariffs_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientAnalyzers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAnalyzers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAnalyzers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Radius = table.Column<double>(type: "float", nullable: false),
                    Area = table.Column<Geometry>(type: "geometry", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAreas_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameOnCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CCV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExpireDate = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientCards_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OutMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BreakInMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BreakOutMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AllowedBreak = table.Column<int>(type: "int", nullable: false),
                    AllowedWorkHour = table.Column<int>(type: "int", nullable: false),
                    AllowedOvertime = table.Column<int>(type: "int", nullable: false),
                    AutoOutCycle = table.Column<int>(type: "int", nullable: false, defaultValue: 4),
                    DefaultFirstOffDay = table.Column<int>(type: "int", nullable: false),
                    DefaultSecondOffDay = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSettings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientUserCaptures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefType = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUserCaptures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUserCaptures_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientUserSettings",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstOffDay = table.Column<int>(type: "int", nullable: false),
                    SecondOffDay = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUserSettings", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_ClientUserSettings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientUserTrainings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUserTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUserTrainings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Engineer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NaCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CellPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianBirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarBirthDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DadName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tell = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdSection = table.Column<int>(type: "int", nullable: false),
                    JulianMembershipDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarMembershipDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldTypeEnum = table.Column<int>(type: "int", nullable: false),
                    EducationTypeEnum = table.Column<int>(type: "int", nullable: false),
                    MaritalStatusTypeEnum = table.Column<int>(type: "int", nullable: false),
                    RelatedTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Additional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultQuota = table.Column<long>(type: "bigint", nullable: false),
                    CertOfTest = table.Column<bool>(type: "bit", nullable: false),
                    CertOfEarth = table.Column<bool>(type: "bit", nullable: false),
                    CertOfFiber = table.Column<bool>(type: "bit", nullable: false),
                    CertOfInspection = table.Column<bool>(type: "bit", nullable: false),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortIndex = table.Column<int>(type: "int", nullable: false),
                    BankAccountBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Has1Percent = table.Column<bool>(type: "bit", nullable: false),
                    HasQuarterIncrease = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engineer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Engineer_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EngPaymentTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SolarCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    FromSolar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromJulian = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToSolar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToJulian = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarPay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianPay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePaymentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngPaymentTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngPaymentTasks_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ErtTariffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErtSystemTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Tariff = table.Column<long>(type: "bigint", nullable: false),
                    Factor = table.Column<float>(type: "real", nullable: false),
                    JulianDateExecute = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarDateExecute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErtTariffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErtTariffs_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Executors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnershipTypeEnum = table.Column<int>(type: "int", nullable: false),
                    ExecutorTypeEnum = table.Column<int>(type: "int", nullable: false),
                    ExecutorGradTypeEnum = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NaCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CellPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    License = table.Column<bool>(type: "bit", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdSection = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoreInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianLicenseExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolarLicenseExpire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Executors_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PanelMakers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignatureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvinceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdSection = table.Column<int>(type: "int", nullable: false),
                    MoreInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanelMakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PanelMakers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Supports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketNumber = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    SolarCreate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JulianCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarEndSupport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianEndSupport = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileNumber = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    Field1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Field2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supports_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GatewayType = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionStatus = table.Column<int>(type: "int", nullable: false),
                    JulianCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserFileTypeEnum = table.Column<int>(type: "int", nullable: false),
                    FolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFiles_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientAreaPoint",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    ClientAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAreaPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAreaPoint_ClientAreas_ClientAreaId",
                        column: x => x.ClientAreaId,
                        principalTable: "ClientAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientUserArea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ClientAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUserArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUserArea_ClientAreas_ClientAreaId",
                        column: x => x.ClientAreaId,
                        principalTable: "ClientAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EngineerHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EngineerGradeTypeEnum = table.Column<int>(type: "int", nullable: false),
                    WorkPermitNum = table.Column<long>(type: "bigint", nullable: false),
                    JulianIssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarIssueDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SolarIssueDateInt = table.Column<int>(type: "int", nullable: false),
                    JulianValidityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarValidityDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolarValidityDateInt = table.Column<int>(type: "int", nullable: false),
                    WorkPermission = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    WorkPermitTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineerHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngineerHistories_Engineer_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EngQuotaBurns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuarterTariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountRemaining = table.Column<long>(type: "bigint", nullable: false),
                    AmountBurning = table.Column<long>(type: "bigint", nullable: false),
                    ErtCountRemaining = table.Column<int>(type: "int", nullable: false),
                    ErtCountBurning = table.Column<int>(type: "int", nullable: false),
                    InspectionDelayFactor = table.Column<double>(type: "float", nullable: false),
                    ErtDelayFactor = table.Column<double>(type: "float", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngQuotaBurns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngQuotaBurns_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EngQuotaBurns_Engineer_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EngQuotaBurns_QuarterTariffs_QuarterTariffId",
                        column: x => x.QuarterTariffId,
                        principalTable: "QuarterTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuarterIncreases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuarterTariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterIncreases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuarterIncreases_Engineer_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuarterIncreases_QuarterTariffs_QuarterTariffId",
                        column: x => x.QuarterTariffId,
                        principalTable: "QuarterTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileTypeEnum = table.Column<int>(type: "int", nullable: false),
                    FolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportFiles_Supports_SupportId",
                        column: x => x.SupportId,
                        principalTable: "Supports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolarCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSend = table.Column<bool>(type: "bit", nullable: false),
                    IsReceive = table.Column<bool>(type: "bit", nullable: false),
                    IsReadByReceiver = table.Column<bool>(type: "bit", nullable: false),
                    SupportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportMessages_Supports_SupportId",
                        column: x => x.SupportId,
                        principalTable: "Supports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EngPaymentLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngPaymentTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountSystem = table.Column<long>(type: "bigint", nullable: false),
                    Deduction1 = table.Column<long>(type: "bigint", nullable: false),
                    Deduction2 = table.Column<long>(type: "bigint", nullable: false),
                    Deduction3 = table.Column<long>(type: "bigint", nullable: false),
                    Deduction4 = table.Column<long>(type: "bigint", nullable: false),
                    Addition1 = table.Column<long>(type: "bigint", nullable: false),
                    Addition2 = table.Column<long>(type: "bigint", nullable: false),
                    SumAmountSystem = table.Column<long>(type: "bigint", nullable: false),
                    SumAmountWithFish = table.Column<long>(type: "bigint", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayByBankReceipt = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngPaymentLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngPaymentLists_EngPaymentTasks_EngPaymentTaskId",
                        column: x => x.EngPaymentTaskId,
                        principalTable: "EngPaymentTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EngPaymentLists_Engineer_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EngPaymentLists_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SectionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElectProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileNumber = table.Column<long>(type: "bigint", nullable: false),
                    ElectRequestNumber = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PanelMakerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Area = table.Column<int>(type: "int", nullable: false),
                    NumberOfFloor = table.Column<int>(type: "int", nullable: false),
                    DesNumberOfFloor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandlordName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandlordNaCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LandlordPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdSection = table.Column<int>(type: "int", nullable: false),
                    IdCity = table.Column<int>(type: "int", nullable: false),
                    IdProvince = table.Column<int>(type: "int", nullable: false),
                    Lat = table.Column<float>(type: "real", nullable: false),
                    Lng = table.Column<float>(type: "real", nullable: false),
                    PanelNeed = table.Column<bool>(type: "bit", nullable: false),
                    PanelSerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanelMakerSubmit = table.Column<bool>(type: "bit", nullable: false),
                    JulianDatePanelSubmit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolarDatePanelSubmit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianDatePanelRegister = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolarDatePanelRegister = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequesterTypeEnum = table.Column<int>(type: "int", nullable: false),
                    ProjectTypeRequestEnum = table.Column<int>(type: "int", nullable: false),
                    ProjectCreatedTypeEnum = table.Column<int>(type: "int", nullable: false),
                    BuildingTariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ErtTariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectLevelEnum = table.Column<int>(type: "int", nullable: false),
                    JulianRegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarRegisterDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JulianDateDeliver = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolarDateDeliver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianDateSendToElectCo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolarDateSendToElectCo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountSendToElectCo = table.Column<int>(type: "int", nullable: false),
                    IsEarthSystem = table.Column<bool>(type: "bit", nullable: false),
                    IsErtTest = table.Column<bool>(type: "bit", nullable: false),
                    IsBuildingInspection = table.Column<bool>(type: "bit", nullable: false),
                    IsTestAndDelivery = table.Column<bool>(type: "bit", nullable: false),
                    NeedElectNetwork = table.Column<bool>(type: "bit", nullable: false),
                    IsOk = table.Column<bool>(type: "bit", nullable: false),
                    IsStop = table.Column<bool>(type: "bit", nullable: false),
                    StopDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Expired = table.Column<bool>(type: "bit", nullable: false),
                    SupervisorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBigProject = table.Column<bool>(type: "bit", nullable: false),
                    AmountPerArea = table.Column<long>(type: "bigint", nullable: false),
                    FoundationElectrodeArea = table.Column<int>(type: "int", nullable: false),
                    IsNeedEb = table.Column<bool>(type: "bit", nullable: false),
                    ElectProjectStatusEnum = table.Column<int>(type: "int", nullable: false),
                    DefectDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefectEng = table.Column<bool>(type: "bit", nullable: false),
                    SolvedDefectEng = table.Column<bool>(type: "bit", nullable: false),
                    JulianDateDefectEng = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolarDateDefectEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianDateSolvedDefectEng = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolarDateSolvedDefectEng = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefectEngDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefectAdminDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasRelatedPermit = table.Column<bool>(type: "bit", nullable: false),
                    HasSupervision = table.Column<bool>(type: "bit", nullable: false),
                    AreaAsBuilt = table.Column<int>(type: "int", nullable: false),
                    BuildingTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectProjects_BuildingTariffs_BuildingTariffId",
                        column: x => x.BuildingTariffId,
                        principalTable: "BuildingTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElectProjects_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElectProjects_ElectProjects_ParentProjectId",
                        column: x => x.ParentProjectId,
                        principalTable: "ElectProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElectProjects_ErtTariffs_ErtTariffId",
                        column: x => x.ErtTariffId,
                        principalTable: "ErtTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElectProjects_PanelMakers_PanelMakerId",
                        column: x => x.PanelMakerId,
                        principalTable: "PanelMakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElectProjects_Sections_IdSection",
                        column: x => x.IdSection,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckListEdcs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SolarChecked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianChecked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckListEdcEnum = table.Column<int>(type: "int", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: true),
                    ResultDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListEdcs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListEdcs_ElectProjects_ElectProjectId",
                        column: x => x.ElectProjectId,
                        principalTable: "ElectProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckListForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SolarChecked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianChecked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectionDesEnum = table.Column<int>(type: "int", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: true),
                    ResultDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListForms_ElectProjects_ElectProjectId",
                        column: x => x.ElectProjectId,
                        principalTable: "ElectProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckListForms_Engineer_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentEngForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchingTypeEnum = table.Column<int>(type: "int", nullable: false),
                    FazNumberEnum = table.Column<int>(type: "int", nullable: false),
                    BranchingCount = table.Column<int>(type: "int", nullable: false),
                    Ampere = table.Column<float>(type: "real", nullable: false),
                    Power = table.Column<float>(type: "real", nullable: false),
                    PowerSum = table.Column<float>(type: "real", nullable: false),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentEngForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentEngForms_ElectProjects_ElectProjectId",
                        column: x => x.ElectProjectId,
                        principalTable: "ElectProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentEngForms_Engineer_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectProjectErtForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ElectProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ElectrodeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtmX = table.Column<double>(type: "float", nullable: false),
                    UtmY = table.Column<double>(type: "float", nullable: false),
                    ConstructionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectrodeUsageTypeEnum = table.Column<int>(type: "int", nullable: false),
                    ElectrodeUsageTypeOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectrodeExecuteTypeEnum = table.Column<int>(type: "int", nullable: false),
                    ElectrodeExecuteTypeOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectrodeTypeEnum = table.Column<int>(type: "int", nullable: false),
                    ElectrodeTypeOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectrodeMaterialTypeEnum = table.Column<int>(type: "int", nullable: false),
                    ElectrodeMaterialTypeOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectrodeLength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectrodeDiameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherElectrodeMeasure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErtBrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErtTesterBrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementHour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temperature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RainfallAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectrodeResistanceValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceLetterNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolidTypeEnum = table.Column<int>(type: "int", nullable: false),
                    WellDiameter = table.Column<double>(type: "float", nullable: false),
                    WellDepth = table.Column<double>(type: "float", nullable: false),
                    RodDiameter = table.Column<double>(type: "float", nullable: false),
                    RodLength = table.Column<double>(type: "float", nullable: false),
                    RodConnectionConductor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RodConnectionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlateSize = table.Column<double>(type: "float", nullable: false),
                    PlateMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlateConnectionConductor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlateConnectionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeshSize = table.Column<double>(type: "float", nullable: false),
                    MeshMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeshConnectionConductor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeshConnectionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WireCrossSection = table.Column<double>(type: "float", nullable: false),
                    WireMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WireLength = table.Column<double>(type: "float", nullable: false),
                    WireConnectionConductor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WireConnectionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeltCrossSection = table.Column<double>(type: "float", nullable: false),
                    BeltMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeltLength = table.Column<double>(type: "float", nullable: false),
                    BeltDepth = table.Column<double>(type: "float", nullable: false),
                    BeltConnectionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasuredResistanceValue = table.Column<double>(type: "float", nullable: false),
                    ExpertReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasurementErrorPercentage = table.Column<double>(type: "float", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectProjectErtForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectProjectErtForms_ElectProjects_ElectProjectId",
                        column: x => x.ElectProjectId,
                        principalTable: "ElectProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectProjectFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Des = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileTypeEnum = table.Column<int>(type: "int", nullable: false),
                    FolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElectProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectProjectFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectProjectFiles_ElectProjects_ElectProjectId",
                        column: x => x.ElectProjectId,
                        principalTable: "ElectProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElectProjectProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectLevelEnum = table.Column<int>(type: "int", nullable: false),
                    BuildingTariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectionStatusEnum = table.Column<int>(type: "int", nullable: false),
                    Defect = table.Column<bool>(type: "bit", nullable: false),
                    Fee = table.Column<long>(type: "bigint", nullable: false),
                    QuarterTariffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JulianRegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarRegisterDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JulianDateDeliverEngineer = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarDateDeliverEngineer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JulianDateDeliverOffice = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolarDateDeliverOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    JulianDateAccepted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SolarDateAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectProjectProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectProjectProcesses_BuildingTariffs_BuildingTariffId",
                        column: x => x.BuildingTariffId,
                        principalTable: "BuildingTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElectProjectProcesses_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElectProjectProcesses_ElectProjects_ElectProjectId",
                        column: x => x.ElectProjectId,
                        principalTable: "ElectProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElectProjectProcesses_Engineer_EngineerId",
                        column: x => x.EngineerId,
                        principalTable: "Engineer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElectProjectProcesses_QuarterTariffs_QuarterTariffId",
                        column: x => x.QuarterTariffId,
                        principalTable: "QuarterTariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JulianCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolarCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ElectProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ElectProjectProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    AmountSupervision = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceStatus = table.Column<int>(type: "int", nullable: false),
                    InvoicePayType = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_ElectProjectProcesses_ElectProjectProcessId",
                        column: x => x.ElectProjectProcessId,
                        principalTable: "ElectProjectProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_ElectProjects_ElectProjectId",
                        column: x => x.ElectProjectId,
                        principalTable: "ElectProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankTransactions_ClientId",
                table: "BankTransactions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingTariffs_ClientId",
                table: "BuildingTariffs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListEdcs_ElectProjectId",
                table: "CheckListEdcs",
                column: "ElectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListForms_ElectProjectId",
                table: "CheckListForms",
                column: "ElectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListForms_EngineerId",
                table: "CheckListForms",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvinceId",
                table: "Cities",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAnalyzers_ClientId",
                table: "ClientAnalyzers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAreaPoint_ClientAreaId",
                table: "ClientAreaPoint",
                column: "ClientAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAreas_ClientId",
                table: "ClientAreas",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCards_CardNumber_Type",
                table: "ClientCards",
                columns: new[] { "CardNumber", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientCards_ClientId",
                table: "ClientCards",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSettings_ClientId",
                table: "ClientSettings",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserArea_ClientAreaId",
                table: "ClientUserArea",
                column: "ClientAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserCaptures_ClientId",
                table: "ClientUserCaptures",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserSettings_ClientId",
                table: "ClientUserSettings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserTrainings_ClientId",
                table: "ClientUserTrainings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEngForms_ElectProjectId",
                table: "CommentEngForms",
                column: "ElectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEngForms_EngineerId",
                table: "CommentEngForms",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjectErtForms_ElectProjectId",
                table: "ElectProjectErtForms",
                column: "ElectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjectFiles_ElectProjectId",
                table: "ElectProjectFiles",
                column: "ElectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjectProcesses_BuildingTariffId",
                table: "ElectProjectProcesses",
                column: "BuildingTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjectProcesses_ClientId",
                table: "ElectProjectProcesses",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjectProcesses_ElectProjectId",
                table: "ElectProjectProcesses",
                column: "ElectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjectProcesses_EngineerId",
                table: "ElectProjectProcesses",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjectProcesses_QuarterTariffId",
                table: "ElectProjectProcesses",
                column: "QuarterTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjects_BuildingTariffId",
                table: "ElectProjects",
                column: "BuildingTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjects_ClientId",
                table: "ElectProjects",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjects_ErtTariffId",
                table: "ElectProjects",
                column: "ErtTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjects_FileNumber",
                table: "ElectProjects",
                column: "FileNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjects_IdSection",
                table: "ElectProjects",
                column: "IdSection");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjects_PanelMakerId",
                table: "ElectProjects",
                column: "PanelMakerId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectProjects_ParentProjectId",
                table: "ElectProjects",
                column: "ParentProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Engineer_ClientId",
                table: "Engineer",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Engineer_NaCode",
                table: "Engineer",
                column: "NaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EngineerHistories_EngineerId",
                table: "EngineerHistories",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_EngPaymentLists_EngineerId",
                table: "EngPaymentLists",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_EngPaymentLists_EngPaymentTaskId",
                table: "EngPaymentLists",
                column: "EngPaymentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_EngPaymentLists_TransactionId",
                table: "EngPaymentLists",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EngPaymentTasks_ClientId",
                table: "EngPaymentTasks",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EngQuotaBurns_ClientId",
                table: "EngQuotaBurns",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EngQuotaBurns_EngineerId",
                table: "EngQuotaBurns",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_EngQuotaBurns_QuarterTariffId",
                table: "EngQuotaBurns",
                column: "QuarterTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_ErtTariffs_ClientId",
                table: "ErtTariffs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Executors_ClientId",
                table: "Executors",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Executors_NaCode",
                table: "Executors",
                column: "NaCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ElectProjectId",
                table: "Invoices",
                column: "ElectProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ElectProjectProcessId",
                table: "Invoices",
                column: "ElectProjectProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TransactionId",
                table: "Invoices",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PanelMakers_ClientId",
                table: "PanelMakers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterIncreases_EngineerId",
                table: "QuarterIncreases",
                column: "EngineerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterIncreases_QuarterTariffId",
                table: "QuarterIncreases",
                column: "QuarterTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ParentId",
                table: "Routes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CityId",
                table: "Sections",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportFiles_SupportId",
                table: "SupportFiles",
                column: "SupportId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_SupportId",
                table: "SupportMessages",
                column: "SupportId");

            migrationBuilder.CreateIndex(
                name: "IX_Supports_ClientId",
                table: "Supports",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ClientId",
                table: "Transactions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_ClientId",
                table: "UserFiles",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankTransactions");

            migrationBuilder.DropTable(
                name: "CheckListEdcs");

            migrationBuilder.DropTable(
                name: "CheckListForms");

            migrationBuilder.DropTable(
                name: "ClientAnalyzers");

            migrationBuilder.DropTable(
                name: "ClientAreaPoint");

            migrationBuilder.DropTable(
                name: "ClientCards");

            migrationBuilder.DropTable(
                name: "ClientSettings");

            migrationBuilder.DropTable(
                name: "ClientUserArea");

            migrationBuilder.DropTable(
                name: "ClientUserCaptures");

            migrationBuilder.DropTable(
                name: "ClientUserSettings");

            migrationBuilder.DropTable(
                name: "ClientUserTrainings");

            migrationBuilder.DropTable(
                name: "CommentEngForms");

            migrationBuilder.DropTable(
                name: "ElectProjectErtForms");

            migrationBuilder.DropTable(
                name: "ElectProjectFiles");

            migrationBuilder.DropTable(
                name: "EngineerHistories");

            migrationBuilder.DropTable(
                name: "EngPaymentLists");

            migrationBuilder.DropTable(
                name: "EngQuotaBurns");

            migrationBuilder.DropTable(
                name: "Executors");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "QuarterIncreases");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "SupportFiles");

            migrationBuilder.DropTable(
                name: "SupportMessages");

            migrationBuilder.DropTable(
                name: "UserFiles");

            migrationBuilder.DropTable(
                name: "ClientAreas");

            migrationBuilder.DropTable(
                name: "EngPaymentTasks");

            migrationBuilder.DropTable(
                name: "ElectProjectProcesses");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Supports");

            migrationBuilder.DropTable(
                name: "ElectProjects");

            migrationBuilder.DropTable(
                name: "Engineer");

            migrationBuilder.DropTable(
                name: "QuarterTariffs");

            migrationBuilder.DropTable(
                name: "BuildingTariffs");

            migrationBuilder.DropTable(
                name: "ErtTariffs");

            migrationBuilder.DropTable(
                name: "PanelMakers");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Provinces");
        }
    }
}
