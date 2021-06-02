using Microsoft.EntityFrameworkCore.Migrations;

namespace IsekaiTechnologies.FacCord.Core.DAL.Migrations
{
    public partial class AddItemMeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durability",
                table: "StorageItems");

            migrationBuilder.DropColumn(
                name: "HasDurability",
                table: "StorageItems");

            migrationBuilder.AddColumn<long>(
                name: "ItemMetaId",
                table: "StorageItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemMetas",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Durability = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMetas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_ItemMetaId",
                table: "StorageItems",
                column: "ItemMetaId");

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_ItemMetas_ItemMetaId",
                table: "StorageItems",
                column: "ItemMetaId",
                principalTable: "ItemMetas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StorageItems_ItemMetas_ItemMetaId",
                table: "StorageItems");

            migrationBuilder.DropTable(
                name: "ItemMetas");

            migrationBuilder.DropIndex(
                name: "IX_StorageItems_ItemMetaId",
                table: "StorageItems");

            migrationBuilder.DropColumn(
                name: "ItemMetaId",
                table: "StorageItems");

            migrationBuilder.AddColumn<long>(
                name: "Durability",
                table: "StorageItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "HasDurability",
                table: "StorageItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
