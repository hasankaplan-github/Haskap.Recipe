using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Utilities.Guids;
using Haskap.Recipe.Domain.CategoryAggregate;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.RecipeAggregate.Exceptions;
using Haskap.Recipe.Domain.Shared.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.RecipeAggregate;
public class Recipe : AggregateRoot, IAuditable, ISoftDeletable, IIsDraft, IHasMultiUser
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsDraft { get; private set; }

    private List<Ingredient> _ingredients = new();
    public IReadOnlyList<Ingredient> Ingredients => _ingredients.AsReadOnly();

    private List<RecipeCategory> _categories = new();
    public IReadOnlyList<RecipeCategory> Categories => _categories.AsReadOnly();

    private List<Step> _steps = new();
    public IReadOnlyList<Step> Steps => _steps.AsReadOnly();
    public Common.File Picture { get; private set; }
    public Slug Slug { get; private set; }



    public Guid? CreatedUserId { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool IsDeleted { get; set; }
    public Guid OwnerUserId { get; set; }

    private Recipe() { }

    public Recipe(Guid id, string name, string? description, bool isDraft, Common.File picture)
        : base(id)
    {
        Id = id;
        SetName(name);
        SetDescription(description);
        IsDraft = isDraft;
        SetPicture(picture);
    }

    public void SetPicture(Common.File picture)
    {
        Guard.Against.Null(picture);

        Picture = picture;
    }

    public void SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.InvalidInput(name, nameof(name), x => x.Length <= RecipeConsts.MaxNameLength);

        Name = name;
        Slug = RecipeAggregate.Slug.Generate(name, 20);
    }

    public void SetDescription(string? description)
    {
        if (description is not null)
        {
            Guard.Against.InvalidInput(description, nameof(description), x => x.Length <= RecipeConsts.MaxDescriptionLength);
        }

        Description = description;
    }

    public void Activate()
    {
        if (Ingredients.Any() == false)
        {
            throw new NoIngredientsException();
        }

        if (Steps.Any() == false)
        {
            throw new NoStepsException();
        }

        IsDraft = false;
    }

    public void MarkAsDraft() 
    { 
        IsDraft = true; 
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    public void AddCategory(Category category)
    {
        Guard.Against.Null(category);

        var recipeCategory = new RecipeCategory(GuidGenerator.CreateSimpleGuid()) { RecipeId = Id, CategoryId = category.Id};
        _categories.Add(recipeCategory);
    }

    public void AddCategory(Guid categoryId)
    {
        var recipeCategory = new RecipeCategory(GuidGenerator.CreateSimpleGuid()) { RecipeId = Id, CategoryId = categoryId };
        _categories.Add(recipeCategory);
    }

    public void AddCategories(IList<Guid> categoryIds)
    {
        Guard.Against.Null(categoryIds);

        foreach (var categoryId in categoryIds)
        {
            AddCategory(categoryId);
        }
    }

    public void RemoveCategory(Category category) 
    { 
        Guard.Against.Null(category);

        var categoryToRemove = _categories
            .Where(x => x.CategoryId == category.Id && x.RecipeId == Id)
            .FirstOrDefault();

        if (categoryToRemove is null)
        {
            return;
        }

        _categories.Remove(categoryToRemove);
    }

    public void RemoveCategories(IList<RecipeCategory> categories)
    {
        Guard.Against.Null(categories);

        var categoryIds = categories.Select(x => x.CategoryId);

        _categories.RemoveAll(x => categoryIds.Contains(x.CategoryId));
    }

    public void AddIngredient(Ingredient ingredient)
    {
        Guard.Against.Null(ingredient);

        _ingredients.Add(ingredient);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        Guard.Against.Null(ingredient);

        _ingredients.Remove(ingredient);
    }

    public void AddStep(Step step)
    {
        Guard.Against.Null(step);

        _steps.Add(step);
    }

    public void RemoveStep(Step step)
    {
        Guard.Against.Null(step);

        var stepOrder = step.StepOrder;

        _steps.Remove(step);

        var stepsToBeReordered = _steps
            .Where(x => x.StepOrder > stepOrder)
            .ToList();

        stepsToBeReordered.ForEach(x => x.SetStepOrder(x.StepOrder - 1));
    }

    public void IncreaseStepOrder(Step step)
    {
        Guard.Against.Null(step);

        var currentStepOrder = step.StepOrder;

        if (_steps.Count == currentStepOrder)
        {
            return;
        }

        var stepToBeDecreased = _steps
            .Where(x => x.StepOrder == currentStepOrder + 1)
            .First();

        step.SetStepOrder(currentStepOrder + 1);

        stepToBeDecreased.SetStepOrder(currentStepOrder);
    }

    public void DecreaseStepOrder(Step step) 
    { 
        Guard.Against.Null(step);

        var currentStepOrder = step.StepOrder;

        if (currentStepOrder == 1)
        {
            return;
        }

        var stepToBeIncreased = _steps
            .Where(x => x.StepOrder == currentStepOrder - 1)
            .First();

        step.SetStepOrder(currentStepOrder - 1);

        stepToBeIncreased.SetStepOrder(currentStepOrder);
    }
}
