using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotherInstitute.Migrations
{
    /// <inheritdoc />
    public partial class InitialFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACADEMICSESSION",
                columns: table => new
                {
                    NAME = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACADEMICSESSION", x => x.NAME);
                });

            migrationBuilder.CreateTable(
                name: "BEDDETAILS",
                columns: table => new
                {
                    BEDNO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BUILDING = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FLOOR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ROOM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BEDDETAILS", x => x.BEDNO);
                });

            migrationBuilder.CreateTable(
                name: "COURSE",
                columns: table => new
                {
                    NAME = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COURSE", x => x.NAME);
                });

            migrationBuilder.CreateTable(
                name: "FEES",
                columns: table => new
                {
                    NAME = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FEES", x => x.NAME);
                });

            migrationBuilder.CreateTable(
                name: "ORGANIZATION",
                columns: table => new
                {
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOGO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REGDNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REGDDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MOBILE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MAILID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TAGLINE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORGANIZATION", x => x.SLNO);
                });

            migrationBuilder.CreateTable(
                name: "STUDENTFEE",
                columns: table => new
                {
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STUDENTID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FEESNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENTFEE", x => x.SLNO);
                });

            migrationBuilder.CreateTable(
                name: "STUDENTINSTALLMENT",
                columns: table => new
                {
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STUDENTID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INSTALLMENTNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENTINSTALLMENT", x => x.SLNO);
                });

            migrationBuilder.CreateTable(
                name: "STUDENTREGD",
                columns: table => new
                {
                    STUDENTID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SESSION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COURSE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BEDNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MOB1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MOB2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOR = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GENDER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CASTE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AADHARNO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BLOODGROUP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IMAGE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COLLEGENAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BOARDNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COLLEGEROLLNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CURRYR = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENTREGD", x => x.STUDENTID);
                });

            migrationBuilder.CreateTable(
                name: "STUDENTSUB",
                columns: table => new
                {
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STUDENTID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SUBJECT = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENTSUB", x => x.SLNO);
                });

            migrationBuilder.CreateTable(
                name: "STUDENTVISITORS",
                columns: table => new
                {
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STUDENTID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RELATION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MOBILE = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STUDENTVISITORS", x => x.SLNO);
                });

            migrationBuilder.CreateTable(
                name: "SUBJECTS",
                columns: table => new
                {
                    NAME = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUBJECTS", x => x.NAME);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    LOGINID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SLNO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REGDDATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PSW = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.LOGINID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACADEMICSESSION");

            migrationBuilder.DropTable(
                name: "BEDDETAILS");

            migrationBuilder.DropTable(
                name: "COURSE");

            migrationBuilder.DropTable(
                name: "FEES");

            migrationBuilder.DropTable(
                name: "ORGANIZATION");

            migrationBuilder.DropTable(
                name: "STUDENTFEE");

            migrationBuilder.DropTable(
                name: "STUDENTINSTALLMENT");

            migrationBuilder.DropTable(
                name: "STUDENTREGD");

            migrationBuilder.DropTable(
                name: "STUDENTSUB");

            migrationBuilder.DropTable(
                name: "STUDENTVISITORS");

            migrationBuilder.DropTable(
                name: "SUBJECTS");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
