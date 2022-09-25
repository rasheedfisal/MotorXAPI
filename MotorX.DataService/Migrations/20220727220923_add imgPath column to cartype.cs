using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorX.DataService.Migrations
{
    public partial class addimgPathcolumntocartype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "Cartype",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "Cartype");
        }
    }
}
