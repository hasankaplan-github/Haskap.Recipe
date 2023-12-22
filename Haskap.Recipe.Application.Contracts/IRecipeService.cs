using Haskap.Recipe.Application.Dtos.Common;
using Haskap.Recipe.Application.Dtos.Recipies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Contracts;
public interface IRecipeService
{
    Task<CreateAsDraftOutputDto> CreateAsDraftAsync(CreateAsDraftInputDto inputDto, CancellationToken cancellationToken);
    Task<RecipeOutputDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task ActivateAsync(Guid id, CancellationToken cancellationToken);
    Task MarkAsDraftAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, UpdateInputDto inputDto, CancellationToken cancellationToken);
    Task DeleteIngredientAsync(Guid recipeId, Guid ingredientId, CancellationToken cancellationToken);
    Task SaveNewIngredientAsync(SaveNewIngredientInputDto inputDto, CancellationToken cancellationToken);
    Task UpdateIngredientAsync(UpdateIngredientInputDto inputDto, CancellationToken cancellationToken);
    Task<int> GetStepCountAsync(Guid recipeId, CancellationToken cancellationToken);
    Task SaveNewStepAsync(SaveNewStepInputDto inputDto, List<FileInputDto> pictureFiles, string webRootPath, CancellationToken cancellationToken);
}
