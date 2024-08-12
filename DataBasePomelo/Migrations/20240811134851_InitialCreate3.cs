using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataBasePomelo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Errors_EnumeErrors_EnumsErrorsId",
                table: "Errors");

            migrationBuilder.DropTable(
                name: "EnumeErrors");

            migrationBuilder.DropIndex(
                name: "IX_Errors_EnumsErrorsId",
                table: "Errors");

            migrationBuilder.DropColumn(
                name: "EnumsErrorsId",
                table: "Errors");

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Errors",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Errors");

            migrationBuilder.AddColumn<Guid>(
                name: "EnumsErrorsId",
                table: "Errors",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "EnumeErrors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumeErrors", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Errors_EnumsErrorsId",
                table: "Errors",
                column: "EnumsErrorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Errors_EnumeErrors_EnumsErrorsId",
                table: "Errors",
                column: "EnumsErrorsId",
                principalTable: "EnumeErrors",
                principalColumn: "Id");
        }
    }
}
