using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class IngredientOutputDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public AmountOutputDto Amount { get; set; }
    public Guid IngredientGroupId { get; set; }
}
