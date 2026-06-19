using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotherInstitute.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeErrorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MarketingStudents");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "MarketingAgents");

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "SchoolMasters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "SchoolMasters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudentName",
                table: "MarketingStudents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MarketingStudents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "MarketingStudents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "MarketingStudents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "InitiatedDate",
                table: "MarketingStudents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Session",
                table: "MarketingStudents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VisitedDate",
                table: "MarketingStudents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ErrorTable",
                columns: table => new
                {
                    SlNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErrorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrowserInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorTable", x => x.SlNo);
                });

            migrationBuilder.CreateTable(
                name: "MarketingVisit",
                columns: table => new
                {
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID = table.Column<int>(type: "int", nullable: false),
                    Agent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketingVisit", x => x.SLNO);
                    table.ForeignKey(
                        name: "FK_MarketingVisit_MarketingStudents_ID",
                        column: x => x.ID,
                        principalTable: "MarketingStudents",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketingVisit_ID",
                table: "MarketingVisit",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorTable");

            migrationBuilder.DropTable(
                name: "MarketingVisit");

            migrationBuilder.DropColumn(
                name: "Block",
                table: "SchoolMasters");

            migrationBuilder.DropColumn(
                name: "District",
                table: "SchoolMasters");

            migrationBuilder.DropColumn(
                name: "InitiatedDate",
                table: "MarketingStudents");

            migrationBuilder.DropColumn(
                name: "Session",
                table: "MarketingStudents");

            migrationBuilder.DropColumn(
                name: "VisitedDate",
                table: "MarketingStudents");

            migrationBuilder.AlterColumn<string>(
                name: "StudentName",
                table: "MarketingStudents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MarketingStudents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "MarketingStudents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "MarketingStudents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MarketingStudents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "MarketingAgents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
