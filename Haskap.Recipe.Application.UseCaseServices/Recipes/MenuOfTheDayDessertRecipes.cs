using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDayDessertRecipes : IMenuOfTheDay
{
    private readonly Guid _dessertCategoryId = Guid.Parse("d86e3066-6047-4128-a1d0-fa092429a6fb");
    private readonly IRecipeSearchService _recipeSearchService;


    public MenuOfTheDayDessertRecipes(IRecipeSearchService recipeSearchService)
    {
        _recipeSearchService = recipeSearchService;
    }

    public async Task<List<RecipeOutputDto>> GetRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        var searchOutputDto = await _recipeSearchService.SearchAsync(new SearchInputDto
        {
            CategoryId = _dessertCategoryId,
            CurrentPageIndex = 1,
            PageSize = count,
            SearchIngredients = ingredients,
            SearchName = null
        }, cancellationToken);

        return searchOutputDto.Recipes;
    }
}
