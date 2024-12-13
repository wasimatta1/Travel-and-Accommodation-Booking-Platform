using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updataroombookingrealtion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRooms_Bookings_BookingID",
                table: "BookingRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingRooms_Rooms_RoomID",
                table: "BookingRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Hotels_HotelID",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRooms_Bookings_BookingID",
                table: "BookingRooms",
                column: "BookingID",
                principalTable: "Bookings",
                principalColumn: "BookingID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRooms_Rooms_RoomID",
                table: "BookingRooms",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Hotels_HotelID",
                table: "Bookings",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "HotelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingRooms_Bookings_BookingID",
                table: "BookingRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingRooms_Rooms_RoomID",
                table: "BookingRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Hotels_HotelID",
                table: "Bookings");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRooms_Bookings_BookingID",
                table: "BookingRooms",
                column: "BookingID",
                principalTable: "Bookings",
                principalColumn: "BookingID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingRooms_Rooms_RoomID",
                table: "BookingRooms",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Hotels_HotelID",
                table: "Bookings",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "HotelID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
