using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Contracts;
public interface IMenuOfTheDay
{
    Task<List<RecipeOutputDto>> GetRecipesAsync(string? ingredients, int count, CancellationToken cancellationToken);
}
