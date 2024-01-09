using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDaySoupRecipes : IMenuOfTheDay
{
    private readonly Guid _soupCategoryId = Guid.Parse("ab851a8b-22de-48c3-be2b-3d4f0f8fd24d");
    private readonly IRecipeSearchService _recipeSearchService;


    public MenuOfTheDaySoupRecipes(IRecipeSearchService recipeSearchService)
    {
        _recipeSearchService = recipeSearchService;
    }

    public async Task<List<RecipeOutputDto>> GetRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        var searchOutputDto = await _recipeSearchService.SearchAsync(new SearchInputDto
        {
            CategoryId = _soupCategoryId,
            CurrentPageIndex = 1,
            PageSize = count,
            SearchIngredients = ingredients,
            SearchName = null
        }, cancellationToken);

        return searchOutputDto.Recipes;
    }
}
