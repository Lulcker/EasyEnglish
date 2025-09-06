using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEnglish.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_UserReminderSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserReminderSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Mode = table.Column<int>(type: "integer", nullable: false),
                    TimeZoneId = table.Column<string>(type: "text", nullable: false),
                    ReminderTimes = table.Column<TimeSpan[]>(type: "interval[]", nullable: true),
                    StartWorkTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    EndWorkTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    PeriodicityTime = table.Column<int>(type: "integer", nullable: true),
                    BackgroundJobId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReminderSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReminderSettings_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserReminderSettings_UserId",
                table: "UserReminderSettings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReminderSettings");
        }
    }
}
