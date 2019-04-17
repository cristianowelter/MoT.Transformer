using Microsoft.EntityFrameworkCore.Migrations;

namespace ModelOfThings.Parser.Migrations
{
    public partial class UmlUserCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UmlUseCaseId",
                table: "MddComponents",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UmlUseCases",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MddApplicationId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UmlUseCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UmlUseCases_MddApplications_MddApplicationId",
                        column: x => x.MddApplicationId,
                        principalTable: "MddApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UmlAssociation",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MemberOwned = table.Column<string>(nullable: true),
                    MemberEnd = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    UmlUseCaseId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UmlAssociation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UmlAssociation_UmlUseCases_UmlUseCaseId",
                        column: x => x.UmlUseCaseId,
                        principalTable: "UmlUseCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MddComponents_UmlUseCaseId",
                table: "MddComponents",
                column: "UmlUseCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UmlAssociation_UmlUseCaseId",
                table: "UmlAssociation",
                column: "UmlUseCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UmlUseCases_MddApplicationId",
                table: "UmlUseCases",
                column: "MddApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MddComponents_UmlUseCases_UmlUseCaseId",
                table: "MddComponents",
                column: "UmlUseCaseId",
                principalTable: "UmlUseCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MddComponents_UmlUseCases_UmlUseCaseId",
                table: "MddComponents");

            migrationBuilder.DropTable(
                name: "UmlAssociation");

            migrationBuilder.DropTable(
                name: "UmlUseCases");

            migrationBuilder.DropIndex(
                name: "IX_MddComponents_UmlUseCaseId",
                table: "MddComponents");

            migrationBuilder.DropColumn(
                name: "UmlUseCaseId",
                table: "MddComponents");
        }
    }
}
