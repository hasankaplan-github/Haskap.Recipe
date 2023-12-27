using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.Migrations
{
    /// <inheritdoc />
    public partial class ViewCount_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "view_count",
                table: "recipe",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "view_count",
                table: "recipe");
        }
    }
}
