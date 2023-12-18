using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Common.DataTable;

public class JqueryDataTableParam
{
    public int Draw { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
    public List<ColumnParam>? Columns { get; set; }
    public SearchParam? Search { get; set; }
    public List<OrderParam>? Order { get; set; }
}
