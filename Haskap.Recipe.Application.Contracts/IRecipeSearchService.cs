using Haskap.Recipe.Application.Dtos.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Contracts;
public interface IRecipeSearchService
{
    Task<SearchOutputDto> SearchAsync(SearchInputDto inputDto, CancellationToken cancellationToken);
}
