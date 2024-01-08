using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDayDessertRecipesService
{
    private readonly IMenuOfTheDayService _menuOfTheDayService;
    private readonly Guid _dessertCategoryId = Guid.Parse("d86e3066-6047-4128-a1d0-fa092429a6fb");

    public MenuOfTheDayDessertRecipesService(IMenuOfTheDayService menuOfTheDayService)
    {
        _menuOfTheDayService = menuOfTheDayService;
    }

    public async Task<List<RecipeOutputDto>> GetDessertRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        return await _menuOfTheDayService.GetRecipesAsync(_dessertCategoryId, ingredients, count, cancellationToken);
    }
}
