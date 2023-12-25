using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class CreateAsDraftInputDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IList<Guid>? CategoryIds { get; set; }
}
