using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDayDinnerRecipes : IMenuOfTheDay
{
    private readonly Guid _dinnerCategoryId = Guid.Parse("c1354fe3-776e-4d21-ad9d-bdd623188031");
    private readonly IRecipeSearchService _recipeSearchService;


    public MenuOfTheDayDinnerRecipes(IRecipeSearchService recipeSearchService)
    {
        _recipeSearchService = recipeSearchService;
    }

    public async Task<List<RecipeOutputDto>> GetRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        var searchOutputDto = await _recipeSearchService.SearchAsync(new SearchInputDto
        {
            CategoryId = _dinnerCategoryId,
            CurrentPageIndex = 1,
            PageSize = count,
            SearchIngredients = ingredients,
            SearchName = null
        }, cancellationToken);

        return searchOutputDto.Recipes;
    }
}
