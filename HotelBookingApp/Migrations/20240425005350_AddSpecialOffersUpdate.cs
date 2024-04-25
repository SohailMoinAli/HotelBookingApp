using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialOffersUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialOffers_Rooms_RoomId",
                table: "SpecialOffers");

            migrationBuilder.DropIndex(
                name: "IX_SpecialOffers_RoomId",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SpecialOffers");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "SpecialOffers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SpecialOffers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "SpecialOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialOffers_RoomId",
                table: "SpecialOffers",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialOffers_Rooms_RoomId",
                table: "SpecialOffers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
