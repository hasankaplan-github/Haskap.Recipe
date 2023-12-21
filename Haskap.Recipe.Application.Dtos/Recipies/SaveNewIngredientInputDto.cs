using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class SaveNewIngredientInputDto
{
    public Guid RecipeId { get; set; }
    public Guid IngredientGroupId { get; set; }
    public string NewIngredientGroupName { get; set; }
    public decimal Amount { get; set; }
    public Guid UnitId { get; set; }
    public string NewUnitName { get; set; }
    public string Name { get; set;}
}
