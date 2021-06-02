using Microsoft.EntityFrameworkCore.Migrations;

namespace IsekaiTechnologies.FacCord.Core.DAL.Migrations
{
    public partial class AddPlayerEmoji : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerEmoji",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerEmoji",
                table: "Profiles");
        }
    }
}
