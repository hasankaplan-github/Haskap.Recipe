using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDayBreakfastRecipes : IMenuOfTheDay
{
    private readonly Guid _breakfastCategoryId = Guid.Parse("0e502998-6a5d-4361-953c-b174ac3bd9e4");
    private readonly IRecipeSearchService _recipeSearchService;
    

    public MenuOfTheDayBreakfastRecipes(IRecipeSearchService recipeSearchService)
    {
        _recipeSearchService = recipeSearchService;
    }

    public async Task<List<RecipeOutputDto>> GetRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        var searchOutputDto = await _recipeSearchService.SearchAsync(new SearchInputDto
        {
            CategoryId = _breakfastCategoryId,
            CurrentPageIndex = 1,
            PageSize = count,
            SearchIngredients = ingredients,
            SearchName = null
        }, cancellationToken);

        return searchOutputDto.Recipes;
    }
}
