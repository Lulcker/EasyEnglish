using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyEnglish.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_IsFavorite_For_Table_Card : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Card",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Card");
        }
    }
}
