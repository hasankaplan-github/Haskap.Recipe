using Haskap.Recipe.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class SearchRecipeOutputDto
{
    public Guid RecipeId { get; set; }
    public string RecipeName { get; set; }
    public DateTime? RecipeCreatedOn { get; set; }
    public FileOutputDto RecipePicture { get; set; }
}
