using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorX.DataService.Migrations
{
    public partial class addemailcolumntocustomerinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "OfferCustomerInfo",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "OfferCustomerInfo");
        }
    }
}
