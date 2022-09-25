using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorX.DataService.Migrations
{
    public partial class updatemispelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarOffer_Cartype_CartTypeId",
                table: "CarOffer");

            migrationBuilder.RenameColumn(
                name: "CartTypeId",
                table: "CarOffer",
                newName: "CarTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CarOffer_CartTypeId",
                table: "CarOffer",
                newName: "IX_CarOffer_CarTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarOffer_Cartype_CarTypeId",
                table: "CarOffer",
                column: "CarTypeId",
                principalTable: "Cartype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarOffer_Cartype_CarTypeId",
                table: "CarOffer");

            migrationBuilder.RenameColumn(
                name: "CarTypeId",
                table: "CarOffer",
                newName: "CartTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_CarOffer_CarTypeId",
                table: "CarOffer",
                newName: "IX_CarOffer_CartTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarOffer_Cartype_CartTypeId",
                table: "CarOffer",
                column: "CartTypeId",
                principalTable: "Cartype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
