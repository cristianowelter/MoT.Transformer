using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelOfThings.Parser.Migrations
{
    public partial class Association : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberOwned",
                table: "UmlAssociations",
                newName: "UmlElementId");

            migrationBuilder.RenameColumn(
                name: "MemberEnd",
                table: "UmlAssociations",
                newName: "Source");

            migrationBuilder.AddColumn<string>(
                name: "UmlElementId",
                table: "UmlUseCases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Destiny",
                table: "UmlAssociations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UmlElementId",
                table: "UmlUseCases");

            migrationBuilder.DropColumn(
                name: "Destiny",
                table: "UmlAssociations");

            migrationBuilder.RenameColumn(
                name: "UmlElementId",
                table: "UmlAssociations",
                newName: "MemberOwned");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "UmlAssociations",
                newName: "MemberEnd");
        }
    }
}
