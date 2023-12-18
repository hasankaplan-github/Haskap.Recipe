using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.IngredientGroupAggregate;
public class IngredientGroup : AggregateRoot
{
    public string? Name { get; set; }

    private IngredientGroup()
    {

    }

    public IngredientGroup(Guid id, string? name = null)
        : base(id)
    {
        Name = name;
    }
}
