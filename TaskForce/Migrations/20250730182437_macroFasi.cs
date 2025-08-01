using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskForce.Migrations
{
    /// <inheritdoc />
    public partial class macroFasi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProgettoId",
                table: "FasiProgetto",
                newName: "MacroFaseId");

            migrationBuilder.CreateTable(
                name: "MacroFasi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgettoId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroFasi", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MacroFasi");

            migrationBuilder.RenameColumn(
                name: "MacroFaseId",
                table: "FasiProgetto",
                newName: "ProgettoId");
        }
    }
}
