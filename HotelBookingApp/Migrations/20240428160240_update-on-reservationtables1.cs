using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingApp.Migrations
{
    /// <inheritdoc />
    public partial class updateonreservationtables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomReservation_Reservations_ReservationId",
                table: "RoomReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomReservation_Rooms_RoomId",
                table: "RoomReservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomReservation",
                table: "RoomReservation");

            migrationBuilder.RenameTable(
                name: "RoomReservation",
                newName: "RoomReservations");

            migrationBuilder.RenameIndex(
                name: "IX_RoomReservation_RoomId",
                table: "RoomReservations",
                newName: "IX_RoomReservations_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomReservation_ReservationId",
                table: "RoomReservations",
                newName: "IX_RoomReservations_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomReservations",
                table: "RoomReservations",
                column: "RoomReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomReservations_Reservations_ReservationId",
                table: "RoomReservations",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "ReservationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomReservations_Rooms_RoomId",
                table: "RoomReservations",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomReservations_Reservations_ReservationId",
                table: "RoomReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomReservations_Rooms_RoomId",
                table: "RoomReservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomReservations",
                table: "RoomReservations");

            migrationBuilder.RenameTable(
                name: "RoomReservations",
                newName: "RoomReservation");

            migrationBuilder.RenameIndex(
                name: "IX_RoomReservations_RoomId",
                table: "RoomReservation",
                newName: "IX_RoomReservation_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomReservations_ReservationId",
                table: "RoomReservation",
                newName: "IX_RoomReservation_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomReservation",
                table: "RoomReservation",
                column: "RoomReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomReservation_Reservations_ReservationId",
                table: "RoomReservation",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "ReservationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomReservation_Rooms_RoomId",
                table: "RoomReservation",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
