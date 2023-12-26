using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class PublicSearchOutputDto
{
    public List<PublicSearchRecipeOutputDto> Recipes { get; set; }
    public int FilteredCount { get; set; }
}
