using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskForce.Migrations
{
    /// <inheritdoc />
    public partial class pause : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pause");
        }
    }
}
