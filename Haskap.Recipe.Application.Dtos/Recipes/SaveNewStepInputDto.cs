using Haskap.Recipe.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class SaveNewStepInputDto
{
    public Guid RecipeId { get; set; }
    public string Instruction { get; set; }
}
