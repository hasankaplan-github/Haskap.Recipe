using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDaySoupRecipesService
{
    private readonly IMenuOfTheDayService _menuOfTheDayService;
    private readonly Guid _soupCategoryId = Guid.Parse("ab851a8b-22de-48c3-be2b-3d4f0f8fd24d");

    public MenuOfTheDaySoupRecipesService(IMenuOfTheDayService menuOfTheDayService)
    {
        _menuOfTheDayService = menuOfTheDayService;
    }

    public async Task<List<RecipeOutputDto>> GetSoupRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        return await _menuOfTheDayService.GetRecipesAsync(_soupCategoryId, ingredients, count, cancellationToken);
    }
}
