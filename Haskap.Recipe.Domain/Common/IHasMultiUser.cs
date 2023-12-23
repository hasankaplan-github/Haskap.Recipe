using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.Common;
public interface IHasMultiUser
{
    Guid OwnerUserId { get; set; }
}
