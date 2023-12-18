using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Haskap.Recipe.Domain.RecipeAggregate.Exceptions;
public class NoStepsException : DomainException
{
    public NoStepsException()
        : base("En az bir adım eklemeden tarifi aktifleştiremezsiniz!", HttpStatusCode.BadRequest)
    {

    }
}
