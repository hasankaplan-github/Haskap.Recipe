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
    public Guid IngredientGroupId { get; set; }
    public IngredientGroup IngredientGroup { get; set; }


    private Ingredient() { }

    public Ingredient(Guid id, string name, string? description, Amount amount, Guid ingredientGroupId)
    {
        Id = id;
        SetName(name);
        Description = description;
        SetAmount(amount);
        IngredientGroupId = ingredientGroupId;
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
}
