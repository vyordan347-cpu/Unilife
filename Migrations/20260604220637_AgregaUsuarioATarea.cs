using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unilife.Migrations
{
    /// <inheritdoc />
    public partial class AgregaUsuarioATarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Tareas",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Tareas");
        }
    }
}
