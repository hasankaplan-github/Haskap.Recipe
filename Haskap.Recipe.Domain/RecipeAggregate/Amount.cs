using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.Recipe.Domain.UnitAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.RecipeAggregate;
public class Amount : ValueObject
{
    public decimal Value { get; private set; }
    public Guid UnitId { get; private set; }
    public Unit Unit { get; private set; }


    private Amount()
    {
        
    }

    public Amount(decimal value, Unit unit)
    {
        Guard.Against.NegativeOrZero(value);
        Guard.Against.Null(unit);

        Value = value;
        Unit = unit;
        UnitId = unit.Id;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value; 
        yield return UnitId;
    }
}
