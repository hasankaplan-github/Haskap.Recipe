using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Accounts;
public class UpdateRolesInputDto
{
    public Guid UserId { get; set; }
    public List<Guid>? CheckedRoles { get; set; }
    public List<Guid>? UncheckedRoles { get; set; }
}