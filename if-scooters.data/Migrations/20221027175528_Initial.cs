using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace if_scooters.data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentedScooters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PricePerMinute = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ScooterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentedScooters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scooters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricePerMinute = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsRented = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scooters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentedScooters");

            migrationBuilder.DropTable(
                name: "Scooters");
        }
    }
}
