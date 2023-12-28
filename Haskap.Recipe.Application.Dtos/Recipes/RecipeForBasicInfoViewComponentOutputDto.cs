using Haskap.Recipe.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class RecipeForBasicInfoViewComponentOutputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<RecipeCategoryOutputDto> RecipeCategories { get; set; }
    public FileOutputDto Picture { get; set; }
}
