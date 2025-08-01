using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskForce.Migrations
{
    /// <inheritdoc />
    public partial class miglioramento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pause");

            migrationBuilder.DropColumn(
                name: "GiorniPrevistiBe",
                table: "FasiProgetto");

            migrationBuilder.RenameColumn(
                name: "GiorniPrevistiUi",
                table: "FasiProgetto",
                newName: "GiorniPrevisti");

            migrationBuilder.AddColumn<int>(
                name: "Stato",
                table: "PreseInCarico",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoFase",
                table: "FasiProgetto",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stato",
                table: "PreseInCarico");

            migrationBuilder.DropColumn(
                name: "TipoFase",
                table: "FasiProgetto");

            migrationBuilder.RenameColumn(
                name: "GiorniPrevisti",
                table: "FasiProgetto",
                newName: "GiorniPrevistiUi");

            migrationBuilder.AddColumn<int>(
                name: "GiorniPrevistiBe",
                table: "FasiProgetto",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pause",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataFine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataInizio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PresaInCaricoId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pause", x => x.Id);
                });
        }
    }
}
