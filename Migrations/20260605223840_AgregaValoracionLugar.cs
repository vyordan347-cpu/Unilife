using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unilife.Migrations
{
    /// <inheritdoc />
    public partial class AgregaValoracionLugar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValoracionesLugar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<string>(type: "TEXT", nullable: false),
                    LugarId = table.Column<int>(type: "INTEGER", nullable: false),
                    Puntuacion = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValoracionesLugar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValoracionesLugar_Lugares_LugarId",
                        column: x => x.LugarId,
                        principalTable: "Lugares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ValoracionesLugar_LugarId",
                table: "ValoracionesLugar",
                column: "LugarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValoracionesLugar");
        }
    }
}
