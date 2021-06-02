using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IsekaiTechnologies.FacCord.Core.DAL.Migrations
{
    public partial class AddProspectorScanCoolDown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastFindMine",
                table: "Profiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ScanCooldown",
                table: "Items",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServerSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordServerId = table.Column<long>(nullable: false),
                    ServerPrefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerSettings");

            migrationBuilder.DropColumn(
                name: "LastFindMine",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ScanCooldown",
                table: "Items");
        }
    }
}
