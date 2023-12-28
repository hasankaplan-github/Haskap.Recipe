using Haskap.Recipe.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class RecipeForStepsViewComponentOutputDto
{
    public Guid Id { get; set; }
    public List<StepOutputDto> Steps { get; set; }
}
