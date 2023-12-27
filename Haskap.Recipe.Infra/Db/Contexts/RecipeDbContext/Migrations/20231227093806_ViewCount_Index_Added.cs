using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.Migrations
{
    /// <inheritdoc />
    public partial class ViewCount_Index_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_recipe_view_count",
                table: "recipe",
                column: "view_count");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_recipe_view_count",
                table: "recipe");
        }
    }
}
