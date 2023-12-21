using Haskap.Recipe.Application.Dtos.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Contracts;
public interface IUnitService
{
    Task<List<UnitOutputDto>> GetAllAsync(CancellationToken cancellationToken);
}
