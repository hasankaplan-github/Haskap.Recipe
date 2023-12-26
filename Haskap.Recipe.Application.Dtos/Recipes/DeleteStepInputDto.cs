using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class DeleteStepInputDto
{
    public Guid RecipeId { get; set; }
    public Guid StepId { get; set; }
}
