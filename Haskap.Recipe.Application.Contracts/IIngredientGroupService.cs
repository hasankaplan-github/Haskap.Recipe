using Haskap.Recipe.Application.Dtos.IngredientGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Contracts;
public interface IIngredientGroupService
{
    Task<List<IngredientGroupOutputDto>> GetAllAsync(CancellationToken cancellationToken);
}
