using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDayBreakfastRecipesService
{
    private readonly IMenuOfTheDayService _menuOfTheDayService;
    private readonly Guid _breakfastCategoryId = Guid.Parse("0e502998-6a5d-4361-953c-b174ac3bd9e4");

    public MenuOfTheDayBreakfastRecipesService(IMenuOfTheDayService menuOfTheDayService)
    {
        _menuOfTheDayService = menuOfTheDayService;
    }

    public async Task<List<RecipeOutputDto>> GetBreakfastRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        return await _menuOfTheDayService.GetRecipesAsync(_breakfastCategoryId, ingredients, count, cancellationToken);
    }
}
