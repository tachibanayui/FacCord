using Microsoft.EntityFrameworkCore.Migrations;

namespace IsekaiTechnologies.FacCord.Core.DAL.Migrations
{
    public partial class AddDimension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DimensionId",
                table: "Items",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_DimensionId",
                table: "Items",
                column: "DimensionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Dimensions_DimensionId",
                table: "Items",
                column: "DimensionId",
                principalTable: "Dimensions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Dimensions_DimensionId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_DimensionId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DimensionId",
                table: "Items");
        }
    }
}
