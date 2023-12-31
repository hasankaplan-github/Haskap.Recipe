﻿using Haskap.DddBase.Domain.Providers;
using Haskap.Recipe.Application.Contracts;
using Haskap.DddBase.Infra.Providers;
using Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext;
using Microsoft.EntityFrameworkCore;
using Haskap.Recipe.Domain;
using Haskap.Recipe.Application.UseCaseServices.Accounts;
using Haskap.DddBase.Infra.Db.Interceptors;
using Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;
using Microsoft.AspNetCore.Authorization;
using Haskap.DddBase.Presentation.CustomAuthorization;
using Haskap.Recipe.Application.UseCaseServices.ViewLevelExceptions;
using Haskap.Recipe.Domain.UserAggregate;
using Haskap.Recipe.Application.UseCaseServices.Roles;
using Haskap.Recipe.Application.UseCaseServices.Categories;
using Haskap.Recipe.Application.UseCaseServices.Recipes;
using Haskap.Recipe.Application.UseCaseServices.Units;
using Haskap.Recipe.Application.UseCaseServices.IngredientGroups;
using Haskap.Recipe.Domain.Providers;
using Haskap.Recipe.Infra.Providers;
using Haskap.Recipe.Infra.Db.Interceptors;
using Haskap.Recipe.Application.UseCaseServices.Mappings;

namespace Haskap.Recipe.Ui.MvcWebUi;

public static class ServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<UserDomainService>();
    }

    public static void AddUseCaseServices(this IServiceCollection services)
    {
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IViewLevelExceptionService, ViewLevelExceptionService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IRecipeService, RecipeService>();
        services.AddTransient<IUnitService, UnitService>();
        services.AddTransient<IIngredientGroupService, IngredientGroupService>();

        services.AddScoped<MenuOfTheDayBreakfastRecipes>();
        services.AddScoped<MenuOfTheDayLunchRecipes>();
        services.AddScoped<MenuOfTheDaySoupRecipes>();
        services.AddScoped<MenuOfTheDayDinnerRecipes>();
        services.AddScoped<MenuOfTheDayDessertRecipes>();
        services.AddScoped<MenuOfTheDayGenerator>();
        services.AddScoped<IRecipeSearchService, RecipeSearchService>();
    }

    public static void AddProviders(this IServiceCollection services)
    {
        services.AddSingleton<ILocalDateTimeProvider, LocalDateTimeProvider>();
        services.AddScoped<IIsDraftGlobalQueryFilterProvider, IsDraftGlobalQueryFilterProvider>();
        services.AddScoped<IMultiUserGlobalQueryFilterProvider, MultiUserGlobalQueryFilterProvider>();
        //services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        //services.AddSingleton<IJwtProvider, JwtProvider>();
    }

    public static void AddEfInterceptors(this IServiceCollection services)
    {
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<AuditHistoryLogSaveChangesInterceptor>();
        services.AddScoped<MultiTenancySaveChangesInterceptor>();
        services.AddScoped<MultiUserSaveChangesInterceptor>();
    }

    public static void AddExternalServices(this IServiceCollection services)
    {
        //services.AddScoped<IOsymExamCalendarParser, OsymExamCalendarParser>();
        //services.AddScoped<AuditHistoryLogSaveChangesInterceptor<Guid?>>();
    }

    public static void AddPersistance(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString("RecipeConnectionString");
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
            options.AddInterceptors(
                serviceProvider.GetRequiredService<MultiTenancySaveChangesInterceptor>(),
                serviceProvider.GetRequiredService<MultiUserSaveChangesInterceptor>(),
                serviceProvider.GetRequiredService<AuditHistoryLogSaveChangesInterceptor>(),
                serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
        });

        services.AddScoped<IRecipeDbContext, AppDbContext>();
    }

    public static void AddCustomAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        var recipePermissionProvider = new RecipePermissionProvider();
        services.AddSingleton<IPermissionProvider>(recipePermissionProvider);
        services.AddAuthorization(recipePermissionProvider.ConfigureAuthorization);
    }
}