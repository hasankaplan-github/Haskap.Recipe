using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.ViewLevelExceptionAggregate;
public class ViewLevelException : AggregateRoot
{
    public string Message { get; set; }
    public string? StackTrace { get; set; }
    public DateTime OccuredOnUtc { get; private set; }


    private ViewLevelException()
    { }

    public ViewLevelException(Guid id)
        : base(id)
    {
        OccuredOnUtc = DateTime.UtcNow;
    }
}
