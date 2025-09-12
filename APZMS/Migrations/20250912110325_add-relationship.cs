using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APZMS.Migrations
{
    /// <inheritdoc />
    public partial class addrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ActivityId",
                table: "Bookings",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Activities_ActivityId",
                table: "Bookings",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_CustomerId",
                table: "Bookings",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Activities_ActivityId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_CustomerId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ActivityId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings");
        }
    }
}
