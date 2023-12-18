using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Haskap.Recipe.Domain.RecipeAggregate.Exceptions;
public class NoIngredientsException : DomainException
{
    public NoIngredientsException()
        : base("En az bir malzeme eklemeden tarifi aktifleştiremezsiniz!", HttpStatusCode.BadRequest)
    {

    }
}
