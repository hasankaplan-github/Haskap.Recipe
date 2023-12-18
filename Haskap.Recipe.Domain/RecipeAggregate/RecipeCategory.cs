using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.RecipeAggregate;
public class RecipeCategory : Entity
{
    public Guid RecipeId { get; set; }
    public Guid CategoryId { get; set; }


    private RecipeCategory()
    {

    }


    public RecipeCategory(Guid id)
        : base(id)
    {
    }
}
