using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipes;
public class MenuOfTheDayGenerator
{
    public async Task<List<RecipeOutputDto>> GenerateAsync(IMenuOfTheDay menuOfTheDay, string? ingredients, int count, CancellationToken cancellationToken)
    {
        return await menuOfTheDay.GetRecipesAsync(ingredients, count, cancellationToken);
    }
}
