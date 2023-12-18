using Haskap.DddBase.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Haskap.Recipe.Domain.UserAggregate.Exceptions;
public class PasswordConfirmationMismatchException : DomainException
{
    public PasswordConfirmationMismatchException()
        : base("Şifre doğrulama eşleşmemektedir!", HttpStatusCode.BadRequest)
    {

    }
}
