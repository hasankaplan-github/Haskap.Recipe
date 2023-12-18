using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.UnitAggregate;
public class Unit : AggregateRoot
{
    public string Name { get; set; }

    private Unit()
    {
        
    }

    public Unit(Guid id, string name)
        : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
    }
}
