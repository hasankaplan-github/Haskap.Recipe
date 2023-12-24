using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Recipe_Picture_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "picture_extension",
                table: "recipe",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "picture_new_name",
                table: "recipe",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "picture_original_name",
                table: "recipe",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "picture_extension",
                table: "recipe");

            migrationBuilder.DropColumn(
                name: "picture_new_name",
                table: "recipe");

            migrationBuilder.DropColumn(
                name: "picture_original_name",
                table: "recipe");
        }
    }
}
