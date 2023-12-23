using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class StepPictureOutputDto
{
    public Guid Id { get; set; }
    public string OriginalName { get; set; }
    public string NewName { get; set; }
    public string? Extension { get; set; }
}
