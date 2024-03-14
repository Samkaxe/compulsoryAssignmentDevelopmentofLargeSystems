using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HistoryService.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OperationType = table.Column<string>(type: "TEXT", nullable: false),
                    Operand1 = table.Column<decimal>(type: "TEXT", nullable: false),
                    Operand2 = table.Column<decimal>(type: "TEXT", nullable: false),
                    Result = table.Column<decimal>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationEntries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationEntries");
        }
    }
}
