using Ardalis.GuardClauses;
using Haskap.Recipe.Domain.IngredientGroupAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.RecipeAggregate;
public class Ingredient : Entity
{
    public Guid RecipeId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; set; }
    public Amount Amount { get; private set; }
    public Guid IngredientGroupId { get; private set; }
    public IngredientGroup IngredientGroup { get; private set; }


    private Ingredient() { }

    public Ingredient(Guid id, string name, string? description, Amount amount, IngredientGroup ingredientGroup)
    {
        Id = id;
        SetName(name);
        Description = description;
        SetAmount(amount);
        SetIngredientGroup(ingredientGroup);
    }

    public void SetName(string name)
    {
        Guard.Against.Null(name);

        Name = name;
    }

    public void SetAmount(Amount amount)
    {
        Guard.Against.Null(amount);

        Amount = amount;
    }

    public void SetIngredientGroup(IngredientGroup ingredientGroup)
    {
        Guard.Against.Null(ingredientGroup);

        IngredientGroup = ingredientGroup;
        IngredientGroupId = ingredientGroup.Id;
    }
}
