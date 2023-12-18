using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Haskap.Recipe.Domain.UserAggregate.Exceptions;
public class CurrentPasswordMismatchException : DomainException
{
    public CurrentPasswordMismatchException()
        : base("Güncel şifreniz doğru değil!", HttpStatusCode.BadRequest)
    {

    }
}
