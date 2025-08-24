using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEnglish.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_CardCollection_CardCollectionId",
                table: "Card");
            
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.Sql("""
                                 INSERT INTO "User" ("Id", "FirstName", "Email", "PasswordHash", "PasswordSalt")
                                 VALUES ('8EFFFC10-EC14-44A2-8968-5EC3CE3C5A58', '+vGUPLvd8rSiao/vqQrxFQ==', '1ncOolJ/SbdSl21UaboDj3712jDCsH1sSaz4ru2Y5oU=', 'h893sldA0vFpxPcuD48R3ncZ6uRvbt3ElBnDNRa4vZE=', '7ZtFeT++5h0kbWsGepYGJQ==')
                                 """);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CardCollection",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("8EFFFC10-EC14-44A2-8968-5EC3CE3C5A58"));
            
            migrationBuilder.CreateIndex(
                name: "IX_CardCollection_UserId",
                table: "CardCollection",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_CardCollection_CardCollectionId",
                table: "Card",
                column: "CardCollectionId",
                principalTable: "CardCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardCollection_User_UserId",
                table: "CardCollection",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_CardCollection_CardCollectionId",
                table: "Card");

            migrationBuilder.DropForeignKey(
                name: "FK_CardCollection_User_UserId",
                table: "CardCollection");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_CardCollection_UserId",
                table: "CardCollection");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CardCollection");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_CardCollection_CardCollectionId",
                table: "Card",
                column: "CardCollectionId",
                principalTable: "CardCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
