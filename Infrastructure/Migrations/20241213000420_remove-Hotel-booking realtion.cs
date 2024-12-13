using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeHotelbookingrealtion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Hotels_HotelID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_HotelID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "HotelID",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelID",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HotelID",
                table: "Bookings",
                column: "HotelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Hotels_HotelID",
                table: "Bookings",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "HotelID");
        }
    }
}
