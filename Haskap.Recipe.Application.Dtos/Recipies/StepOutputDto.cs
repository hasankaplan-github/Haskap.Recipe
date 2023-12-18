using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class StepOutputDto
{
    public Guid Id { get; set; }
    public Guid RecipeId { get; set; }
    public string Instruction { get; set; }
    public int StepOrder { get; set; }
    private List<StepPictureOutputDto> Pictures { get; set; }
}
