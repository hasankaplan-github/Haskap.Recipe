using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class PublicSearchInputDto
{
    public string SearchName { get; set; } 
    public string SearchIngredients {  get; set; }
    public int CurrentPageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 1;
}
