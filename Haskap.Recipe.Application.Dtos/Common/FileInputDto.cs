using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Common;

public class FileInputDto
{
    public long ContentLength { get; set; }
    public byte[] Content { get; set; }

    public string OriginalName { get; set; }
    public string NewName { get; set; }
    public string? Extension { get; set; }
}
