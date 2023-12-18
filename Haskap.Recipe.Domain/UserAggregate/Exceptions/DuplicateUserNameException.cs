using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Haskap.Recipe.Domain.UserAggregate.Exceptions;
public class DuplicateUserNameException : DomainException
{
    public DuplicateUserNameException()
        : base("Bu kullanıcı adı başka birisi tarafından kullanılıyor, lütfen başka bir kullanıcı adı seçiniz.", HttpStatusCode.BadRequest)
    {

    }
}
