using AutoMapper;
using Haskap.DddBase.Utilities.Guids;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Recipies;
using Haskap.Recipe.Domain;
using Haskap.Recipe.Domain.RecipeAggregate;
using Haskap.Recipe.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Recipies;
public class RecipeService : IRecipeService
{
    private readonly IRecipeDbContext _recipeDbContext;
    private readonly IMapper _mapper;

    public RecipeService(
        IRecipeDbContext recipeDbContext,
        IMapper mapper)
    {
        _recipeDbContext = recipeDbContext;
        _mapper = mapper;
    }

    public async Task ActivateAsync(Guid id, CancellationToken cancellationToken)
    {
        var recipe = await _recipeDbContext.Recipe
            .Include(x => x.Steps)
            .Include(x => x.Ingredients)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        recipe.Activate();

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkAsDraftAsync(Guid id, CancellationToken cancellationToken)
    {
        var recipe = await _recipeDbContext.Recipe
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        recipe.MarkAsDraft();

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<CreateAsDraftOutputDto> CreateAsDraftAsync(CreateAsDraftInputDto inputDto, CancellationToken cancellationToken)
    {
        var recipe = new Recipe.Domain.RecipeAggregate.Recipe(
            GuidGenerator.CreateSimpleGuid(),
            inputDto.Name,
            inputDto.Description,
            isDraft: true);

        foreach (var categoryId in (inputDto.CategoryIds ?? Enumerable.Empty<Guid>()))
        {
            recipe.AddCategory(categoryId);
        }

        await _recipeDbContext.Recipe.AddAsync(recipe);
        await _recipeDbContext.SaveChangesAsync();

        return new CreateAsDraftOutputDto { Id = recipe.Id };
    }

    public async Task<RecipeOutputDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var recipe = await _recipeDbContext.Recipe
            .Include(x => x.Categories)
            .Include(x => x.Ingredients)
            .Include(x => x.Steps)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        var output = _mapper.Map<RecipeOutputDto>(recipe);
        
        return output;
    }

    public async Task UpdateAsync(Guid id, UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var recipe = await _recipeDbContext.Recipe
            .Include(x => x.Categories)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        recipe.SetName(inputDto.Name);
        recipe.SetDescription(inputDto.Description);

        var toBeDeleted = recipe.Categories
            .IntersectBy(inputDto.UnselectedCategoryIds ?? Enumerable.Empty<Guid>(), x => x.CategoryId)
            .ToList();

        recipe.RemoveCategories(toBeDeleted);



        var toBeAdded = (inputDto.SelectedCategoryIds ?? Enumerable.Empty<Guid>())
            .Except(recipe.Categories.Select(x => x.CategoryId))
            .ToList();

        recipe.AddCategories(toBeAdded);

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

}
