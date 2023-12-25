using Haskap.Recipe.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class UpdateStepInputDto
{
    public Guid RecipeId { get; set; }
    public Guid StepId { get; set; }
    public string Instruction { get; set; }
    public List<Guid>? DeletedPictureIds { get; set; }
}
