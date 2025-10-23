using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEnglish.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_IsLearned_For_Table_CardCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLearned",
                table: "CardCollection",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLearned",
                table: "CardCollection");
        }
    }
}
