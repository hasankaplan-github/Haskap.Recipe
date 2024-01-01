using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class MenuOfTheDayOutputDto
{
    public RecipeOutputDto? BreakfastRecipe { get; set; }
    public RecipeOutputDto? LunchRecipe { get; set; }
    public RecipeOutputDto? SoupRecipe { get; set; }
    public RecipeOutputDto? DinnerRecipe { get; set; }
}
