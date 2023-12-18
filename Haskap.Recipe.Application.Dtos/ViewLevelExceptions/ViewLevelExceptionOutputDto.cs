using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.ViewLevelExceptions;
public class ViewLevelExceptionOutputDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string? StackTrace { get; set; }
    public DateTime OccuredOnUtc { get; set; }
}
