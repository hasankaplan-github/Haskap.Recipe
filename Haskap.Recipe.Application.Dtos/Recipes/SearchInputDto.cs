using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class SearchInputDto
{
    public string? SearchName { get; set; } 
    public string? SearchIngredients {  get; set; }
    public int CurrentPageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 9;
    public Guid? CategoryId { get; set; } = null;
}
