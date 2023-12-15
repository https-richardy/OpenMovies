using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenMovies.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContainsSpoilerAndClassificationFieldsInReviewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Classification",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ContainsSpoiler",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classification",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ContainsSpoiler",
                table: "Reviews");
        }
    }
}
