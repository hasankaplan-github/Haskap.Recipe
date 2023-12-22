using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "picture_original_name",
                table: "step_picture",
                newName: "file_original_name");

            migrationBuilder.RenameColumn(
                name: "picture_new_name",
                table: "step_picture",
                newName: "file_new_name");

            migrationBuilder.RenameColumn(
                name: "picture_extension",
                table: "step_picture",
                newName: "file_extension");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "ingredient_group",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "file_original_name",
                table: "step_picture",
                newName: "picture_original_name");

            migrationBuilder.RenameColumn(
                name: "file_new_name",
                table: "step_picture",
                newName: "picture_new_name");

            migrationBuilder.RenameColumn(
                name: "file_extension",
                table: "step_picture",
                newName: "picture_extension");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "ingredient_group",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
