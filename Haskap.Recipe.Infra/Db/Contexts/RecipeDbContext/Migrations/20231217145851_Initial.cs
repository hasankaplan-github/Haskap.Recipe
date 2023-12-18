using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_history_log",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    visit_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modification_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modification_type = table.Column<string>(type: "text", nullable: false),
                    object_full_type = table.Column<string>(type: "text", nullable: false),
                    object_ids = table.Column<string>(type: "text", nullable: true),
                    object_original_values = table.Column<string>(type: "text", nullable: true),
                    object_new_values = table.Column<string>(type: "text", nullable: true),
                    ownership_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_history_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parent_category_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_category_category_category_id",
                        column: x => x.parent_category_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingredient_group",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingredient_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "recipe",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    is_draft = table.Column<bool>(type: "boolean", nullable: false),
                    created_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unit",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unit", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    credentials_user_name = table.Column<string>(type: "text", nullable: false),
                    credentials_password_hashed_value = table.Column<string>(type: "text", nullable: false),
                    credentials_password_salt_value = table.Column<string>(type: "text", nullable: false),
                    system_time_zone_id = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "view_level_exception",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    stack_trace = table.Column<string>(type: "text", nullable: true),
                    occured_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_view_level_exception", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "recipe_category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipe_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_recipe_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_recipe_category_category_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_recipe_category_recipe_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "step",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipe_id = table.Column<Guid>(type: "uuid", nullable: false),
                    instruction = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    step_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_step", x => x.id);
                    table.ForeignKey(
                        name: "fk_step_recipe_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_permissions_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ingredient",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    recipe_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    amount_value = table.Column<decimal>(type: "numeric", nullable: false),
                    amount_unit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ingredient_group_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingredient", x => x.id);
                    table.ForeignKey(
                        name: "fk_ingredient_ingredient_group_ingredient_group_id",
                        column: x => x.ingredient_group_id,
                        principalTable: "ingredient_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ingredient_recipe_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ingredient_unit_amount_unit_id",
                        column: x => x.amount_unit_id,
                        principalTable: "unit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_permissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_permissions_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_role_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "step_picture",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    picture_original_name = table.Column<string>(type: "text", nullable: false),
                    picture_new_name = table.Column<string>(type: "text", nullable: false),
                    picture_extension = table.Column<string>(type: "text", nullable: true),
                    step_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_step_picture", x => x.id);
                    table.ForeignKey(
                        name: "fk_step_picture_step_step_id",
                        column: x => x.step_id,
                        principalTable: "step",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_category_parent_category_id",
                table: "category",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingredient_amount_unit_id",
                table: "ingredient",
                column: "amount_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingredient_ingredient_group_id",
                table: "ingredient",
                column: "ingredient_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingredient_recipe_id",
                table: "ingredient",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_category_category_id",
                table: "recipe_category",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_recipe_category_recipe_id_category_id",
                table: "recipe_category",
                columns: new[] { "recipe_id", "category_id" });

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_role_id",
                table: "role_permissions",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_step_recipe_id",
                table: "step",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "ix_step_picture_step_id",
                table: "step_picture",
                column: "step_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_credentials_user_name",
                table: "user",
                column: "credentials_user_name");

            migrationBuilder.CreateIndex(
                name: "ix_user_permissions_user_id",
                table: "user_permissions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_user_id",
                table: "user_role",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_history_log");

            migrationBuilder.DropTable(
                name: "ingredient");

            migrationBuilder.DropTable(
                name: "recipe_category");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "step_picture");

            migrationBuilder.DropTable(
                name: "user_permissions");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "view_level_exception");

            migrationBuilder.DropTable(
                name: "ingredient_group");

            migrationBuilder.DropTable(
                name: "unit");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "step");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "recipe");
        }
    }
}
