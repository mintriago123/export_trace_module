using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExportModule.Migrations
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
                    Endpoint = table.Column<string>(type: "text", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultasAPI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cultivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultivos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TareasProgramadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FechaEjecucion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareasProgramadas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plagas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Nivel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ConsultaAPIId = table.Column<int>(type: "integer", nullable: false),
                    CultivoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plagas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plagas_ConsultasAPI_ConsultaAPIId",
                        column: x => x.ConsultaAPIId,
                        principalTable: "ConsultasAPI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plagas_Cultivos_CultivoId",
                        column: x => x.CultivoId,
                        principalTable: "Cultivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DatosAExportar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaExportacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    TipoDato = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Formato = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DatosAExportar_Cultivos_CultivoId",
                        column: x => x.CultivoId,
                        principalTable: "Cultivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DatosAExportar_Plagas_PlagaId",
                        column: x => x.PlagaId,
                        principalTable: "Plagas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatosAExportar_ConsultaAPIId",
                table: "DatosAExportar",
                column: "ConsultaAPIId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosAExportar_CultivoId",
                table: "DatosAExportar",
                column: "CultivoId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosAExportar_PlagaId",
                table: "DatosAExportar",
                column: "PlagaId");

            migrationBuilder.CreateIndex(
                name: "IX_Plagas_ConsultaAPIId",
                table: "Plagas",
                column: "ConsultaAPIId");

            migrationBuilder.CreateIndex(
                name: "IX_Plagas_CultivoId",
                table: "Plagas",
                column: "CultivoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatosAExportar");

            migrationBuilder.DropTable(
                name: "TareasProgramadas");

            migrationBuilder.DropTable(
                name: "Plagas");

            migrationBuilder.DropTable(
                name: "ConsultasAPI");

            migrationBuilder.DropTable(
                name: "Cultivos");
        }
    }
}
