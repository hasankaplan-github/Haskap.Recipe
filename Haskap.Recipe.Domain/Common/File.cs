using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Utilities.Guids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.Common;
public class File : ValueObject
{
    public Guid Id { get; init; }

    public string OriginalName { get; private set; }
    public string NewName { get; private set; }
    public string? Extension { get; private set; }

    private File()
    {

    }

    public File(string originalName)
    {
        Guard.Against.NullOrWhiteSpace(originalName);

        OriginalName = originalName; // Path.GetFileNameWithoutExtension(fileName);
        Extension = Path.GetExtension(originalName);
        NewName = GuidGenerator.CreateSimpleGuid().ToString();
    }

    public File(string originalName, string newName, string? extension)
    {
        Guard.Against.NullOrWhiteSpace(originalName);
        Guard.Against.NullOrWhiteSpace(newName);

        OriginalName = originalName;
        NewName = newName;
        Extension = extension;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return OriginalName;
        yield return NewName;
        yield return Extension ?? string.Empty;
    }
}
