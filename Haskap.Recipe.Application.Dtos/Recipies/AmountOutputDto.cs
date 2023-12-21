using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haskap.Recipe.Application.Dtos.Units;

namespace Haskap.Recipe.Application.Dtos.Recipies;
public class AmountOutputDto
{
    public decimal Value { get; set; }
    public Guid UnitId { get; set; }
    public UnitOutputDto Unit { get; set; }
}
