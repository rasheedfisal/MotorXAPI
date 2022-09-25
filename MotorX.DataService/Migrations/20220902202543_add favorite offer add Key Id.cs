using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorX.DataService.Migrations
{
    public partial class addfavoriteofferaddKeyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteOffer",
                table: "FavoriteOffer");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FavoriteOffer",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "FavoriteOffer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteOffer",
                table: "FavoriteOffer",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteOffer",
                table: "FavoriteOffer");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FavoriteOffer");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FavoriteOffer",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteOffer",
                table: "FavoriteOffer",
                column: "UserId");
        }
    }
}
