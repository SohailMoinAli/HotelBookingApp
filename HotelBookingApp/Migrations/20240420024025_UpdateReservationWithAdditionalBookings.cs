using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationWithAdditionalBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdditionalBookings_ReservationId",
                table: "AdditionalBookings");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalBookings_ReservationId",
                table: "AdditionalBookings",
                column: "ReservationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AdditionalBookings_ReservationId",
                table: "AdditionalBookings");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalBookings_ReservationId",
                table: "AdditionalBookings",
                column: "ReservationId",
                unique: true);
        }
    }
}
