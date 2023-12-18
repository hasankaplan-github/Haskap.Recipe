using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Accounts;
public class UpdatePermissionsInputDto
{
    public Guid UserId { get; set; }
    public List<string> CheckedPermissions { get; set; }
    public List<string> UncheckedPermissions { get; set; }
}