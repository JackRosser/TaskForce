using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskForce.Migrations
{
    /// <inheritdoc />
    public partial class servizi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GiorniPrevistiUi",
                table: "FasiProgetto",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GiorniPrevistiBe",
                table: "FasiProgetto",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Stato",
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
                table: "FasiProgetto");

            migrationBuilder.AlterColumn<int>(
                name: "GiorniPrevistiUi",
                table: "FasiProgetto",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GiorniPrevistiBe",
                table: "FasiProgetto",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
