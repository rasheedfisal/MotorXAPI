using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorX.DataService.Migrations
{
    public partial class addfavoriteoffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavoriteOffer_OfferId",
                table: "FavoriteOffer");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteOffer_OfferId",
                table: "FavoriteOffer",
                column: "OfferId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FavoriteOffer_OfferId",
                table: "FavoriteOffer");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteOffer_OfferId",
                table: "FavoriteOffer",
                column: "OfferId");
        }
    }
}
