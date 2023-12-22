﻿using AutoMapper;
using Haskap.DddBase.Utilities.Guids;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Common;
using Haskap.Recipe.Application.Dtos.Recipies;
using Haskap.Recipe.Domain;
using Haskap.Recipe.Domain.IngredientGroupAggregate;
using Haskap.Recipe.Domain.RecipeAggregate;
using Haskap.Recipe.Domain.RecipeAggregate.Events;
using Haskap.Recipe.Domain.UnitAggregate;
using Haskap.Recipe.Domain.UserAggregate;
using Haskap.Recipe.Domain.UserAggregate.Exceptions;
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
            .ThenInclude(x => x.IngredientGroup)
            .Include(x => x.Ingredients)
            .ThenInclude(x => x.Amount.Unit)
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

    public async Task DeleteIngredientAsync(Guid recipeId, Guid ingredientId, CancellationToken cancellationToken)
    {
        var recipe = await _recipeDbContext.Recipe
            .Include(x => x.Ingredients.Where(y => y.Id == ingredientId))
            .Where(x => x.Id == recipeId)
            .FirstAsync(cancellationToken);

        recipe.RemoveIngredient(recipe.Ingredients.First());

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveNewIngredientAsync(SaveNewIngredientInputDto inputDto, CancellationToken cancellationToken)
    {
        var ingredientGroup = await _recipeDbContext.IngredientGroup
            .Where(x => x.Id == inputDto.IngredientGroupId)
            .FirstOrDefaultAsync(cancellationToken);

        if (ingredientGroup is null)
        {
            ingredientGroup = new IngredientGroup(GuidGenerator.CreateSimpleGuid(), inputDto.NewIngredientGroupName);
            await _recipeDbContext.IngredientGroup.AddAsync(ingredientGroup, cancellationToken);
        }

        var unit = await _recipeDbContext.Unit
            .Where(x => x.Id == inputDto.UnitId)
            .FirstOrDefaultAsync(cancellationToken);

        if (unit is null)
        {
            unit = new Unit(GuidGenerator.CreateSimpleGuid(), inputDto.NewUnitName);
            await _recipeDbContext.Unit.AddAsync(unit, cancellationToken);
        }

        var amount = new Amount(inputDto.Amount, unit);

        var newIngredient = new Ingredient(
            GuidGenerator.CreateSimpleGuid(),
            inputDto.Name,
            null,
            amount,
            ingredientGroup);

        var recipe = await _recipeDbContext.Recipe
            .Where(x => x.Id == inputDto.RecipeId)
            .FirstOrDefaultAsync(cancellationToken);

        recipe.AddIngredient(newIngredient);

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateIngredientAsync(UpdateIngredientInputDto inputDto, CancellationToken cancellationToken)
    {
        var ingredientGroup = await _recipeDbContext.IngredientGroup
            .Where(x => x.Id == inputDto.IngredientGroupId)
            .FirstOrDefaultAsync(cancellationToken);

        if (ingredientGroup is null)
        {
            ingredientGroup = new IngredientGroup(GuidGenerator.CreateSimpleGuid(), inputDto.NewIngredientGroupName);
            await _recipeDbContext.IngredientGroup.AddAsync(ingredientGroup, cancellationToken);
        }

        var unit = await _recipeDbContext.Unit
            .Where(x => x.Id == inputDto.UnitId)
            .FirstOrDefaultAsync(cancellationToken);

        if (unit is null)
        {
            unit = new Unit(GuidGenerator.CreateSimpleGuid(), inputDto.NewUnitName);
            await _recipeDbContext.Unit.AddAsync(unit, cancellationToken);
        }

        var amount = new Amount(inputDto.Amount, unit);

        var recipe = await _recipeDbContext.Recipe
            .Include(x => x.Ingredients.Where(y => y.Id == inputDto.IngredientId))
            .Where(x => x.Id == inputDto.RecipeId)
            .FirstOrDefaultAsync(cancellationToken);

        var ingredientToBeUpdated = recipe.Ingredients.First();

        ingredientToBeUpdated.SetName(inputDto.Name);
        ingredientToBeUpdated.SetAmount(amount);
        ingredientToBeUpdated.SetIngredientGroup(ingredientGroup);

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetStepCountAsync(Guid recipeId, CancellationToken cancellationToken)
    {
        var recipe = await _recipeDbContext.Recipe
            .Include(x => x.Steps)
            .Where(x => x.Id == recipeId)
            .FirstOrDefaultAsync(cancellationToken);

        var stepCount = recipe.Steps.Count();

        return stepCount;
    }

    public async Task SaveNewStepAsync(SaveNewStepInputDto inputDto, List<FileInputDto> pictureFiles, string webRootPath, CancellationToken cancellationToken)
    {
        var stepPictures = new List<StepPicture>();

        foreach (var pictureFile in pictureFiles)
        {
            var stepPicture = new StepPicture(pictureFile.OriginalName);
            stepPictures.Add(stepPicture);

            pictureFile.NewName = stepPicture.File.NewName;
            pictureFile.Extension = stepPicture.File.Extension;
        }

        var newStepOrder = (await GetStepCountAsync(inputDto.RecipeId, cancellationToken)) + 1;

        var newStep = new Step(
            GuidGenerator.CreateSimpleGuid(),
            inputDto.Instruction,
            newStepOrder,
            stepPictures);

        var recipe = await _recipeDbContext.Recipe
            .Where(x => x.Id == inputDto.RecipeId)
            .FirstOrDefaultAsync(cancellationToken);

        recipe.AddStep(newStep);

        await _recipeDbContext.SaveChangesAsync(cancellationToken);

        await MediatorWrapper.Publish(new StepCreatedDomainEvent(inputDto.RecipeId, newStep.Id, pictureFiles, webRootPath), cancellationToken);
    }

    public async Task IncreaseStepOrderAsync(IncreaseStepOrderInputDto inputDto, CancellationToken cancellationToken)
    {
        var recipe = await _recipeDbContext.Recipe
            .Include(x => x.Steps)
            .Where(x => x.Id == inputDto.RecipeId)
            .FirstAsync(cancellationToken);

        var stepToBeIncreased = recipe.Steps
            .Where(x => x.Id == inputDto.StepId)
            .First();

        recipe.IncreaseStepOrder(stepToBeIncreased);
        
        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DecreaseStepOrderAsync(DecreaseStepOrderInputDto inputDto, CancellationToken cancellationToken)
    {
        var recipe = await _recipeDbContext.Recipe
            .Include(x => x.Steps)
            .Where(x => x.Id == inputDto.RecipeId)
            .FirstAsync(cancellationToken);

        var stepToBeDecreased = recipe.Steps
            .Where(x => x.Id == inputDto.StepId)
            .First();

        recipe.DecreaseStepOrder(stepToBeDecreased);

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteStepAsync(DeleteStepInputDto inputDto, string webRootPath, CancellationToken cancellationToken)
    {
        var recipe = await _recipeDbContext.Recipe
            .Include(x => x.Steps)
            .Where(x => x.Id == inputDto.RecipeId)
            .FirstOrDefaultAsync(cancellationToken);

        var stepToBeRemoved = recipe.Steps
            .Where(x => x.Id == inputDto.StepId)
            .First();

        recipe.RemoveStep(stepToBeRemoved);

        await _recipeDbContext.SaveChangesAsync(cancellationToken);

        await MediatorWrapper.Publish(new StepDeletedDomainEvent(inputDto.RecipeId, inputDto.StepId, webRootPath), cancellationToken);
    }
}
