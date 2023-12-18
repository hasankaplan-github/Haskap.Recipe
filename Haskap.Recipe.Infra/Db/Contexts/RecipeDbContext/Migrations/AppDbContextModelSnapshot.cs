﻿// <auto-generated />
using System;
using Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Haskap.DddBase.Domain.AuditHistoryLogAggregate.AuditHistoryLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("ModificationDateUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modification_date_utc");

                    b.Property<string>("ModificationType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("modification_type");

                    b.Property<Guid?>("ModifiedUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_user_id");

                    b.Property<string>("ObjectFullType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("object_full_type");

                    b.Property<string>("ObjectIds")
                        .HasColumnType("text")
                        .HasColumnName("object_ids");

                    b.Property<string>("ObjectNewValues")
                        .HasColumnType("text")
                        .HasColumnName("object_new_values");

                    b.Property<string>("ObjectOriginalValues")
                        .HasColumnType("text")
                        .HasColumnName("object_original_values");

                    b.Property<string>("OwnershipType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ownership_type");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uuid")
                        .HasColumnName("tenant_id");

                    b.Property<Guid?>("VisitId")
                        .HasColumnType("uuid")
                        .HasColumnName("visit_id");

                    b.HasKey("Id")
                        .HasName("pk_audit_history_log");

                    b.ToTable("audit_history_log", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.CategoryAggregate.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<Guid?>("ParentCategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_category_id");

                    b.HasKey("Id")
                        .HasName("pk_category");

                    b.HasIndex("ParentCategoryId")
                        .HasDatabaseName("ix_category_parent_category_id");

                    b.ToTable("category", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.IngredientGroupAggregate.IngredientGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_ingredient_group");

                    b.ToTable("ingredient_group", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RecipeAggregate.Ingredient", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("description");

                    b.Property<Guid>("IngredientGroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("ingredient_group_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uuid")
                        .HasColumnName("recipe_id");

                    b.HasKey("Id")
                        .HasName("pk_ingredient");

                    b.HasIndex("IngredientGroupId")
                        .HasDatabaseName("ix_ingredient_ingredient_group_id");

                    b.HasIndex("RecipeId")
                        .HasDatabaseName("ix_ingredient_recipe_id");

                    b.ToTable("ingredient", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RecipeAggregate.Recipe", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<Guid?>("CreatedUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("created_user_id");

                    b.Property<string>("Description")
                        .HasMaxLength(1500)
                        .HasColumnType("character varying(1500)")
                        .HasColumnName("description");

                    b.Property<bool>("IsDraft")
                        .HasColumnType("boolean")
                        .HasColumnName("is_draft");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_on");

                    b.Property<Guid?>("ModifiedUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("modified_user_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_recipe");

                    b.ToTable("recipe", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RecipeAggregate.RecipeCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uuid")
                        .HasColumnName("recipe_id");

                    b.HasKey("Id")
                        .HasName("pk_recipe_category");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_recipe_category_category_id");

                    b.HasIndex("RecipeId", "CategoryId")
                        .HasDatabaseName("ix_recipe_category_recipe_id_category_id");

                    b.ToTable("recipe_category", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RecipeAggregate.Step", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Instruction")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)")
                        .HasColumnName("instruction");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uuid")
                        .HasColumnName("recipe_id");

                    b.Property<int>("StepOrder")
                        .HasColumnType("integer")
                        .HasColumnName("step_order");

                    b.HasKey("Id")
                        .HasName("pk_step");

                    b.HasIndex("RecipeId")
                        .HasDatabaseName("ix_step_recipe_id");

                    b.ToTable("step", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RoleAggregate.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_role");

                    b.ToTable("role", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.UnitAggregate.Unit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_unit");

                    b.ToTable("unit", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("SystemTimeZoneId")
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("system_time_zone_id");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.UserAggregate.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_role");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_role_user_id");

                    b.ToTable("user_role", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.ViewLevelExceptionAggregate.ViewLevelException", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.Property<DateTime>("OccuredOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("occured_on_utc");

                    b.Property<string>("StackTrace")
                        .HasColumnType("text")
                        .HasColumnName("stack_trace");

                    b.HasKey("Id")
                        .HasName("pk_view_level_exception");

                    b.ToTable("view_level_exception", (string)null);
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.CategoryAggregate.Category", b =>
                {
                    b.HasOne("Haskap.Recipe.Domain.CategoryAggregate.Category", null)
                        .WithMany("ChildCategories")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_category_category_category_id");
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RecipeAggregate.Ingredient", b =>
                {
                    b.HasOne("Haskap.Recipe.Domain.IngredientGroupAggregate.IngredientGroup", "IngredientGroup")
                        .WithMany()
                        .HasForeignKey("IngredientGroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_ingredient_ingredient_group_ingredient_group_id");

                    b.HasOne("Haskap.Recipe.Domain.RecipeAggregate.Recipe", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_ingredient_recipe_recipe_id");

                    b.OwnsOne("Haskap.Recipe.Domain.RecipeAggregate.Amount", "Amount", b1 =>
                        {
                            b1.Property<Guid>("IngredientId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<Guid>("UnitId")
                                .HasColumnType("uuid")
                                .HasColumnName("amount_unit_id");

                            b1.Property<decimal>("Value")
                                .HasColumnType("numeric")
                                .HasColumnName("amount_value");

                            b1.HasKey("IngredientId");

                            b1.HasIndex("UnitId")
                                .HasDatabaseName("ix_ingredient_amount_unit_id");

                            b1.ToTable("ingredient");

                            b1.WithOwner()
                                .HasForeignKey("IngredientId")
                                .HasConstraintName("fk_ingredient_ingredient_id");

                            b1.HasOne("Haskap.Recipe.Domain.UnitAggregate.Unit", "Unit")
                                .WithMany()
                                .HasForeignKey("UnitId")
                                .OnDelete(DeleteBehavior.Restrict)
                                .IsRequired()
                                .HasConstraintName("fk_ingredient_unit_amount_unit_id");

                            b1.Navigation("Unit");
                        });

                    b.Navigation("Amount")
                        .IsRequired();

                    b.Navigation("IngredientGroup");
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RecipeAggregate.RecipeCategory", b =>
                {
                    b.HasOne("Haskap.Recipe.Domain.CategoryAggregate.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_recipe_category_category_category_id");

                    b.HasOne("Haskap.Recipe.Domain.RecipeAggregate.Recipe", null)
                        .WithMany("Categories")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_recipe_category_recipe_recipe_id");
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RecipeAggregate.Step", b =>
                {
                    b.HasOne("Haskap.Recipe.Domain.RecipeAggregate.Recipe", null)
                        .WithMany("Steps")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_step_recipe_recipe_id");

                    b.OwnsMany("Haskap.Recipe.Domain.RecipeAggregate.StepPicture", "Pictures", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<Guid>("StepId")
                                .HasColumnType("uuid")
                                .HasColumnName("step_id");

                            b1.HasKey("Id")
                                .HasName("pk_step_picture");

                            b1.HasIndex("StepId")
                                .HasDatabaseName("ix_step_picture_step_id");

                            b1.ToTable("step_picture", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("StepId")
                                .HasConstraintName("fk_step_picture_step_step_id");

                            b1.OwnsOne("Haskap.Recipe.Domain.Common.File", "Picture", b2 =>
                                {
                                    b2.Property<Guid>("StepPictureId")
                                        .HasColumnType("uuid")
                                        .HasColumnName("id");

                                    b2.Property<string>("Extension")
                                        .HasColumnType("text")
                                        .HasColumnName("picture_extension");

                                    b2.Property<string>("NewName")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("picture_new_name");

                                    b2.Property<string>("OriginalName")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("picture_original_name");

                                    b2.HasKey("StepPictureId");

                                    b2.ToTable("step_picture");

                                    b2.WithOwner()
                                        .HasForeignKey("StepPictureId")
                                        .HasConstraintName("fk_step_picture_step_picture_id");
                                });

                            b1.Navigation("Picture")
                                .IsRequired();
                        });

                    b.Navigation("Pictures");
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RoleAggregate.Role", b =>
                {
                    b.OwnsMany("Haskap.Recipe.Domain.Common.Permission", "Permissions", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("name");

                            b1.Property<Guid>("RoleId")
                                .HasColumnType("uuid")
                                .HasColumnName("role_id");

                            b1.HasKey("Id")
                                .HasName("pk_role_permissions");

                            b1.HasIndex("RoleId")
                                .HasDatabaseName("ix_role_permissions_role_id");

                            b1.ToTable("role_permissions", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RoleId")
                                .HasConstraintName("fk_role_permissions_role_role_id");
                        });

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.UserAggregate.User", b =>
                {
                    b.OwnsOne("Haskap.Recipe.Domain.UserAggregate.Credentials", "Credentials", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("UserName")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("credentials_user_name");

                            b1.HasKey("UserId");

                            b1.HasIndex("UserName")
                                .HasDatabaseName("ix_user_credentials_user_name");

                            b1.ToTable("user");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_user_user_id");

                            b1.OwnsOne("Haskap.Recipe.Domain.UserAggregate.Password", "Password", b2 =>
                                {
                                    b2.Property<Guid>("CredentialsUserId")
                                        .HasColumnType("uuid")
                                        .HasColumnName("id");

                                    b2.Property<string>("HashedValue")
                                        .IsRequired()
                                        .HasColumnType("text")
                                        .HasColumnName("credentials_password_hashed_value");

                                    b2.HasKey("CredentialsUserId");

                                    b2.ToTable("user");

                                    b2.WithOwner()
                                        .HasForeignKey("CredentialsUserId")
                                        .HasConstraintName("fk_user_user_id");

                                    b2.OwnsOne("Haskap.Recipe.Domain.UserAggregate.Salt", "Salt", b3 =>
                                        {
                                            b3.Property<Guid>("PasswordCredentialsUserId")
                                                .HasColumnType("uuid")
                                                .HasColumnName("id");

                                            b3.Property<string>("Value")
                                                .IsRequired()
                                                .HasColumnType("text")
                                                .HasColumnName("credentials_password_salt_value");

                                            b3.HasKey("PasswordCredentialsUserId");

                                            b3.ToTable("user");

                                            b3.WithOwner()
                                                .HasForeignKey("PasswordCredentialsUserId")
                                                .HasConstraintName("fk_user_user_id");
                                        });

                                    b2.Navigation("Salt")
                                        .IsRequired();
                                });

                            b1.Navigation("Password")
                                .IsRequired();
                        });

                    b.OwnsMany("Haskap.Recipe.Domain.Common.Permission", "Permissions", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("name");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid")
                                .HasColumnName("user_id");

                            b1.HasKey("Id")
                                .HasName("pk_user_permissions");

                            b1.HasIndex("UserId")
                                .HasDatabaseName("ix_user_permissions_user_id");

                            b1.ToTable("user_permissions", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_user_permissions_user_user_id");
                        });

                    b.Navigation("Credentials")
                        .IsRequired();

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.UserAggregate.UserRole", b =>
                {
                    b.HasOne("Haskap.Recipe.Domain.UserAggregate.User", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_role_user_user_id");
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.CategoryAggregate.Category", b =>
                {
                    b.Navigation("ChildCategories");
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.RecipeAggregate.Recipe", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Ingredients");

                    b.Navigation("Steps");
                });

            modelBuilder.Entity("Haskap.Recipe.Domain.UserAggregate.User", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
