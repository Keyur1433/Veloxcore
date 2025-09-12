using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APZMS.Migrations
{
    /// <inheritdoc />
    public partial class classlibrarymodifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookingListItems",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Participants = table.Column<int>(type: "int", nullable: false),
                    CustomerBirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingListItems");
        }
    }
}
