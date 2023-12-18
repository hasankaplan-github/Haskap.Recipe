using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Common.DataTable;

public class JqueryDataTableResult
{
    // properties are not capital due to json mapping
    public int draw { get; set; }
    public int recordsTotal { get; set; }
    public int recordsFiltered { get; set; }
    public dynamic data { get; set; }
}
