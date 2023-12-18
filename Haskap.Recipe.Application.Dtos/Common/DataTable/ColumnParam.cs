using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Common.DataTable;

public class ColumnParam
{
    public string Data { get; set; }
    public string Name { get; set; }
    public bool Searchable { get; set; }
    public bool Orderable { get; set; }
    public SearchParam Search { get; set; }
}
