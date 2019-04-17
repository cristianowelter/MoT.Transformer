using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelOfThings.Parser.Migrations
{
    public partial class UmlAssociation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UmlAssociation_UmlUseCases_UmlUseCaseId",
                table: "UmlAssociation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmlAssociation",
                table: "UmlAssociation");

            migrationBuilder.RenameTable(
                name: "UmlAssociation",
                newName: "UmlAssociations");

            migrationBuilder.RenameIndex(
                name: "IX_UmlAssociation_UmlUseCaseId",
                table: "UmlAssociations",
                newName: "IX_UmlAssociations_UmlUseCaseId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "UmlAssociations",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmlAssociations",
                table: "UmlAssociations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UmlAssociations_UmlUseCases_UmlUseCaseId",
                table: "UmlAssociations",
                column: "UmlUseCaseId",
                principalTable: "UmlUseCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UmlAssociations_UmlUseCases_UmlUseCaseId",
                table: "UmlAssociations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UmlAssociations",
                table: "UmlAssociations");

            migrationBuilder.RenameTable(
                name: "UmlAssociations",
                newName: "UmlAssociation");

            migrationBuilder.RenameIndex(
                name: "IX_UmlAssociations_UmlUseCaseId",
                table: "UmlAssociation",
                newName: "IX_UmlAssociation_UmlUseCaseId");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "UmlAssociation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UmlAssociation",
                table: "UmlAssociation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UmlAssociation_UmlUseCases_UmlUseCaseId",
                table: "UmlAssociation",
                column: "UmlUseCaseId",
                principalTable: "UmlUseCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
