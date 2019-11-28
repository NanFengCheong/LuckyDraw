using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuckyDraw.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    WWID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.WWID);
                });

            migrationBuilder.CreateTable(
                name: "Prizes",
                columns: table => new
                {
                    PrizeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrizeName = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    WWID = table.Column<string>(nullable: true),
                    DrawTime = table.Column<DateTimeOffset>(nullable: true),
                    CollectTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prizes", x => x.PrizeID);
                    table.ForeignKey(
                        name: "FK_Prizes_Employees_WWID",
                        column: x => x.WWID,
                        principalTable: "Employees",
                        principalColumn: "WWID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_WWID",
                table: "Prizes",
                column: "WWID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prizes");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
