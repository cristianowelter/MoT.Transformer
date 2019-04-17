using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelOfThings.Parser.Migrations
{
    public partial class ConnectionOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConnectionIn",
                table: "MddComponents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ConnectionOut",
                table: "MddComponents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionIn",
                table: "MddComponents");

            migrationBuilder.DropColumn(
                name: "ConnectionOut",
                table: "MddComponents");
        }
    }
}
