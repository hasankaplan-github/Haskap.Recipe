﻿using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDayLunchRecipes : IMenuOfTheDay
{
    private readonly Guid _lunchCategoryId = Guid.Parse("ee0b4b1e-8e15-471b-8970-d83360cb52dd");
    private readonly IRecipeSearchService _recipeSearchService;


    public MenuOfTheDayLunchRecipes(IRecipeSearchService recipeSearchService)
    {
        _recipeSearchService = recipeSearchService;
    }

    public async Task<List<RecipeOutputDto>> GetRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        var searchOutputDto = await _recipeSearchService.SearchAsync(new SearchInputDto
        {
            CategoryId = _lunchCategoryId,
            CurrentPageIndex = 1,
            PageSize = count,
            SearchIngredients = ingredients,
            SearchName = null
        }, cancellationToken);

        return searchOutputDto.Recipes;
    }
}
