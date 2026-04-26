using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unilife.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarEventosLugares : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interesados",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Eventos",
                newName: "TipoEvento");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Lugares",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Lugares",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CarreraId",
                table: "Eventos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Eventos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Lugares");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Lugares");

            migrationBuilder.DropColumn(
                name: "CarreraId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "TipoEvento",
                table: "Eventos",
                newName: "Tipo");

            migrationBuilder.AddColumn<int>(
                name: "Interesados",
                table: "Eventos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
