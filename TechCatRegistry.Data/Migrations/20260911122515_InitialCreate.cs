using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechCatRegistry.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catalog",
                columns: table => new
                {
                    CatalogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PublishedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.CatalogId);
                });

            migrationBuilder.CreateTable(
                name: "EstimateType",
                columns: table => new
                {
                    EstimateTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimateCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimateType", x => x.EstimateTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ParameterGroup",
                columns: table => new
                {
                    ParameterGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterGroup", x => x.ParameterGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    ComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogId = table.Column<int>(type: "int", nullable: false),
                    SheetCode = table.Column<string>(type: "nvarchar(31)", maxLength: 31, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Component_Catalog_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalog",
                        principalColumn: "CatalogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parameter",
                columns: table => new
                {
                    ParameterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameter", x => x.ParameterId);
                    table.ForeignKey(
                        name: "FK_Parameter_ParameterGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ParameterGroup",
                        principalColumn: "ParameterGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataPoint",
                columns: table => new
                {
                    DataPointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentId = table.Column<int>(type: "int", nullable: false),
                    ParameterId = table.Column<int>(type: "int", nullable: false),
                    EstimateTypeId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    NumericValue = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    TxtValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PriceYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataPoint", x => x.DataPointId);
                    table.CheckConstraint("CK_DataPoint_ExactlyOneValue", "([NumericValue] IS NULL AND [TxtValue] IS NOT NULL) OR ([TxtValue] IS NULL AND [NumericValue] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_DataPoint_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataPoint_EstimateType_EstimateTypeId",
                        column: x => x.EstimateTypeId,
                        principalTable: "EstimateType",
                        principalColumn: "EstimateTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataPoint_Parameter_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameter",
                        principalColumn: "ParameterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EstimateType",
                columns: new[] { "EstimateTypeId", "EstimateCode" },
                values: new object[,]
                {
                    { 1, "ctrl" },
                    { 2, "lower" },
                    { 3, "upper" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Component_CatalogId_SheetCode",
                table: "Component",
                columns: new[] { "CatalogId", "SheetCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataPoint_ComponentId_ParameterId_EstimateTypeId_Year",
                table: "DataPoint",
                columns: new[] { "ComponentId", "ParameterId", "EstimateTypeId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DataPoint_EstimateTypeId",
                table: "DataPoint",
                column: "EstimateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataPoint_ParameterId",
                table: "DataPoint",
                column: "ParameterId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameter_GroupId_Name",
                table: "Parameter",
                columns: new[] { "GroupId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataPoint");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "EstimateType");

            migrationBuilder.DropTable(
                name: "Parameter");

            migrationBuilder.DropTable(
                name: "Catalog");

            migrationBuilder.DropTable(
                name: "ParameterGroup");
        }
    }
}
