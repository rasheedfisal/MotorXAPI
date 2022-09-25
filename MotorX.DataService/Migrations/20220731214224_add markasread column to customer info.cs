using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorX.DataService.Migrations
{
    public partial class addmarkasreadcolumntocustomerinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MarkAsRead",
                table: "OfferCustomerInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkAsRead",
                table: "OfferCustomerInfo");
        }
    }
}
