using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAgendamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cancelado",
                table: "Agendamentos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MotivoCancelamento",
                table: "Agendamentos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelado",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "MotivoCancelamento",
                table: "Agendamentos");
        }
    }
}
