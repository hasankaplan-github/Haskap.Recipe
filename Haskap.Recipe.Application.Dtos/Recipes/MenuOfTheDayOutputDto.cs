using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class MenuOfTheDayOutputDto
{
    public List<RecipeOutputDto> BreakfastRecipes { get; set; }
    public List<RecipeOutputDto> LunchRecipes { get; set; }
    public List<RecipeOutputDto> SoupRecipes { get; set; }
    public List<RecipeOutputDto> DinnerRecipes { get; set; }
    public List<RecipeOutputDto> DessertRecipes { get; set; }
}
