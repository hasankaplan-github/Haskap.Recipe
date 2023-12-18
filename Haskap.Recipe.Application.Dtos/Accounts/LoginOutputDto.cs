using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Accounts;
public class LoginOutputDto
{
    public Guid UserId { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string? UserSystemTimeZoneId { get; set; }
}
