using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorX.DataService.Migrations
{
    public partial class addcartypecolumntocaroffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CartTypeId",
                table: "CarOffer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CarOffer_CartTypeId",
                table: "CarOffer",
                column: "CartTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarOffer_Cartype_CartTypeId",
                table: "CarOffer",
                column: "CartTypeId",
                principalTable: "Cartype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarOffer_Cartype_CartTypeId",
                table: "CarOffer");

            migrationBuilder.DropIndex(
                name: "IX_CarOffer_CartTypeId",
                table: "CarOffer");

            migrationBuilder.DropColumn(
                name: "CartTypeId",
                table: "CarOffer");
        }
    }
}
