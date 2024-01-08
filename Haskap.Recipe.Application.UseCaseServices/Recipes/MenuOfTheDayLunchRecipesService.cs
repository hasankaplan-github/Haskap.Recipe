using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDayLunchRecipesService
{
    private readonly IMenuOfTheDayService _menuOfTheDayService;
    private readonly Guid _lunchCategoryId = Guid.Parse("ee0b4b1e-8e15-471b-8970-d83360cb52dd");

    public MenuOfTheDayLunchRecipesService(IMenuOfTheDayService menuOfTheDayService)
    {
        _menuOfTheDayService = menuOfTheDayService;
    }

    public async Task<List<RecipeOutputDto>> GetLunchRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        return await _menuOfTheDayService.GetRecipesAsync(_lunchCategoryId, ingredients, count, cancellationToken);
    }
}
