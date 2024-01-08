using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDayDinnerRecipesService
{
    private readonly IMenuOfTheDayService _menuOfTheDayService;
    private readonly Guid _dinnerCategoryId = Guid.Parse("c1354fe3-776e-4d21-ad9d-bdd623188031");

    public MenuOfTheDayDinnerRecipesService(IMenuOfTheDayService menuOfTheDayService)
    {
        _menuOfTheDayService = menuOfTheDayService;
    }

    public async Task<List<RecipeOutputDto>> GetDinnerRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken)
    {
        return await _menuOfTheDayService.GetRecipesAsync(_dinnerCategoryId, ingredients, count, cancellationToken);
    }
}
