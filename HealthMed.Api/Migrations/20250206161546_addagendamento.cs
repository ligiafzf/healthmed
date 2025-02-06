using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Api.Migrations
{
    /// <inheritdoc />
    public partial class addagendamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Pacientes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Pacientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
