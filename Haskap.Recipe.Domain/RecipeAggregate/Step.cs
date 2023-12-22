using Ardalis.GuardClauses;
using Haskap.Recipe.Domain.Shared.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.RecipeAggregate;
public class Step : Entity
{
    public Guid RecipeId { get; private set; }
    public string Instruction { get; private set; }
    public int StepOrder { get; private set; }

    private List<StepPicture> _pictures = new();
    public IReadOnlyList<StepPicture> Pictures => _pictures.AsReadOnly();


    private Step() { }

    public Step(Guid id, string instruction, int stepOrder, List<StepPicture> pictures)
        : base(id)
    {
        Guard.Against.NullOrWhiteSpace(instruction);
        Guard.Against.InvalidInput(instruction, nameof(instruction), x => x.Length <= StepConsts.MaxInstructionLength);

        Id = id;
        Instruction = instruction;
        SetStepOrder(stepOrder);
        AddPictures(pictures);
    }

    public void AddPictures(IEnumerable<StepPicture> pictures)
    {
        Guard.Against.Null(pictures);

        _pictures.AddRange(pictures);
    }

    public void AddPicture(StepPicture picture) => _pictures.Add(picture);

    public void RemovePicture(StepPicture picture) => _pictures.Remove(picture);

    public void SetStepOrder(int stepOrder)
    {
        Guard.Against.NegativeOrZero(stepOrder);

        StepOrder = stepOrder;
    }
}
