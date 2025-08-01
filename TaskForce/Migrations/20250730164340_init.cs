using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskForce.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FasiProgetto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgettoId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GiorniPrevistiBe = table.Column<int>(type: "int", nullable: false),
                    GiorniPrevistiUi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasiProgetto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pause",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PresaInCaricoId = table.Column<int>(type: "int", nullable: false),
                    DataInizio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFine = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pause", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreseInCarico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaseProgettoId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DataPresaInCarico = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFineLavoro = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreseInCarico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Progetti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Consegna = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progetti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FasiProgetto");

            migrationBuilder.DropTable(
                name: "Pause");

            migrationBuilder.DropTable(
                name: "PreseInCarico");

            migrationBuilder.DropTable(
                name: "Progetti");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
