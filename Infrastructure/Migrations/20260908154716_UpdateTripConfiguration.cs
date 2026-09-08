using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTripConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Trips_IsDeleted_Status_DisplayOrder",
                table: "Trips",
                columns: new[] { "IsDeleted", "Status", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_IsFeatured",
                table: "Trips",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_Status",
                table: "Trips",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_Status_DisplayOrder",
                table: "Trips",
                columns: new[] { "Status", "DisplayOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trips_IsDeleted_Status_DisplayOrder",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_IsFeatured",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_Status",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_Status_DisplayOrder",
                table: "Trips");
        }
    }
}
