using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Utilities.Guids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.RecipeAggregate;
public class StepPicture : ValueObject
{
    public Guid Id { get; init; }

    public Common.File Picture { get; private set; }


    private StepPicture()
    {
        
    }

    public StepPicture(string originalName)
    {
        Guard.Against.NullOrWhiteSpace(originalName);

        Picture = new Common.File(originalName);
    }

    public StepPicture(string originalName, string newName, string? extension)
    {
        Guard.Against.NullOrWhiteSpace(originalName);
        Guard.Against.NullOrWhiteSpace(newName);

        Picture = new Common.File(originalName, newName, extension);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Picture;
    }
}
