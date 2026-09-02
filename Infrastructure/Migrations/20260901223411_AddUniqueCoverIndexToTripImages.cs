using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueCoverIndexToTripImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TripImages_TripId",
                table: "TripImages");

            migrationBuilder.CreateIndex(
                name: "IX_TripImages_TripId_IsCover_Unique",
                table: "TripImages",
                column: "TripId",
                unique: true,
                filter: "[IsCover] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TripImages_TripId_IsCover_Unique",
                table: "TripImages");

            migrationBuilder.CreateIndex(
                name: "IX_TripImages_TripId",
                table: "TripImages",
                column: "TripId");
        }
    }
}
