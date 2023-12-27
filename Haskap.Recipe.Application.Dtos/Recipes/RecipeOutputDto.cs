using Haskap.Recipe.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.Dtos.Recipes;
public class RecipeOutputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsDraft { get; set; }
    public List<IngredientOutputDto> Ingredients { get; set; }
    public List<RecipeCategoryOutputDto> Categories { get; set; }
    public List<StepOutputDto> Steps { get; set; }
    public FileOutputDto Picture { get; set; }
    public SlugOutputDto Slug { get; set; }
    public long ViewCount { get; set; }
    public short Rating { get; set; } = 0;

    public Guid? CreatedUserId { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public Guid OwnerUserId { get; set; }
    public string OwnerUserUsername { get; set; }
}
