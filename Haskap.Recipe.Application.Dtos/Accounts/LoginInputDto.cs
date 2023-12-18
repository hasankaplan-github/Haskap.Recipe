using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Accounts;
public class LoginInputDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? ReturnUrl { get; set; } = null;
}
