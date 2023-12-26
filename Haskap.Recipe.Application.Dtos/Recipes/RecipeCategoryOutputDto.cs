using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class RecipeCategoryOutputDto
{
    public Guid RecipeId { get; set; }
    public Guid CategoryId { get; set; }
}
