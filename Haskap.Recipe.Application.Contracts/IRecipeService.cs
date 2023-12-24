using Haskap.Recipe.Application.Dtos.Common;
using Haskap.Recipe.Application.Dtos.Common.DataTable;
using Haskap.Recipe.Application.Dtos.Recipies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Contracts;
public interface IRecipeService
{
    Task<CreateAsDraftOutputDto> CreateAsDraftAsync(CreateAsDraftInputDto inputDto, FileInputDto pictureFile, string webRootPath, CancellationToken cancellationToken);
    Task<RecipeOutputDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task ActivateAsync(Guid id, CancellationToken cancellationToken);
    Task MarkAsDraftAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, UpdateInputDto inputDto, FileInputDto pictureFile, string webRootPath, CancellationToken cancellationToken);
    Task DeleteIngredientAsync(Guid recipeId, Guid ingredientId, CancellationToken cancellationToken);
    Task SaveNewIngredientAsync(SaveNewIngredientInputDto inputDto, CancellationToken cancellationToken);
    Task UpdateIngredientAsync(UpdateIngredientInputDto inputDto, CancellationToken cancellationToken);
    Task<int> GetStepCountAsync(Guid recipeId, CancellationToken cancellationToken);
    Task SaveNewStepAsync(SaveNewStepInputDto inputDto, List<FileInputDto> pictureFiles, string webRootPath, CancellationToken cancellationToken);
    Task IncreaseStepOrderAsync(IncreaseStepOrderInputDto inputDto, CancellationToken cancellationToken);
    Task DecreaseStepOrderAsync(DecreaseStepOrderInputDto inputDto, CancellationToken cancellationToken);
    Task DeleteStepAsync(DeleteStepInputDto inputDto, string webRootPath, CancellationToken cancellationToken);
    Task MarkAsDeletedAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateStepAsync(UpdateStepInputDto inputDto, List<FileInputDto> pictureFiles, string webRootPath, CancellationToken cancellationToken);
    Task<JqueryDataTableResult> EditorSearchAsync(EditorSearchInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken);
}
