using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Haskap.Recipe.Domain.RoleAggregate.Exceptions;
public class DuplicateRoleNameException : DomainException
{
    public DuplicateRoleNameException()
        : base("Bu isim kayıtlı, farklı bir isim deneyin!", HttpStatusCode.BadRequest)
    {

    }
}
