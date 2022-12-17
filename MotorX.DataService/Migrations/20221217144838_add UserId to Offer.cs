using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorX.DataService.Migrations
{
    public partial class addUserIdtoOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CarOffer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarOffer_UserId",
                table: "CarOffer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarOffer_AspNetUsers_UserId",
                table: "CarOffer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarOffer_AspNetUsers_UserId",
                table: "CarOffer");

            migrationBuilder.DropIndex(
                name: "IX_CarOffer_UserId",
                table: "CarOffer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CarOffer");
        }
    }
}
