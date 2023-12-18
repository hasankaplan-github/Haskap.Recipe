using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class UpdateInputDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IList<Guid> SelectedCategoryIds { get; set; }
    public IList<Guid> UnselectedCategoryIds { get; set; }
}
