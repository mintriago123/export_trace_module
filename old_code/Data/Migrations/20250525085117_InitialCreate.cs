using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Export_trace_module.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsultasAPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Endpoint = table.Column<string>(type: "varchar(255)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultasAPI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatosAExportar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaExportacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TipoDato = table.Column<string>(type: "varchar(50)", nullable: false),
                    Formato = table.Column<string>(type: "varchar(10)", nullable: false),
                    Contenido = table.Column<string>(type: "text", nullable: true),
                    CultivoId = table.Column<int>(type: "integer", nullable: true),
                    PlagaId = table.Column<int>(type: "integer", nullable: true),
                    ConsultaAPIId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosAExportar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatosAExportar_ConsultasAPI_ConsultaAPIId",
                        column: x => x.ConsultaAPIId,
                        principalTable: "ConsultasAPI",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Plaga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Nivel = table.Column<string>(type: "text", nullable: false),
                    NombreCientifico = table.Column<string>(type: "text", nullable: true),
                    TipoPlaga = table.Column<string>(type: "text", nullable: true),
                    Sintomas = table.Column<string>(type: "text", nullable: true),
                    TratamientoRecomendado = table.Column<string>(type: "text", nullable: true),
                    CultivoId = table.Column<int>(type: "integer", nullable: false),
                    CultivoNombre = table.Column<string>(type: "text", nullable: true),
                    DetectadaPorPrimeraVez = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EsEndemica = table.Column<bool>(type: "boolean", nullable: true),
                    Fuente = table.Column<string>(type: "text", nullable: true),
                    ConsultaAPIId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plaga", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plaga_ConsultasAPI_ConsultaAPIId",
                        column: x => x.ConsultaAPIId,
                        principalTable: "ConsultasAPI",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsultasAPI_Fecha",
                table: "ConsultasAPI",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_DatosAExportar_ConsultaAPIId",
                table: "DatosAExportar",
                column: "ConsultaAPIId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosAExportar_FechaExportacion",
                table: "DatosAExportar",
                column: "FechaExportacion");

            migrationBuilder.CreateIndex(
                name: "IX_DatosAExportar_TipoDato",
                table: "DatosAExportar",
                column: "TipoDato");

            migrationBuilder.CreateIndex(
                name: "IX_Plaga_ConsultaAPIId",
                table: "Plaga",
                column: "ConsultaAPIId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatosAExportar");

            migrationBuilder.DropTable(
                name: "Plaga");

            migrationBuilder.DropTable(
                name: "ConsultasAPI");
        }
    }
}
