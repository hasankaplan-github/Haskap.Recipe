using AutoMapper;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.IngredientGroups;
using Haskap.Recipe.Application.Dtos.Units;
using Haskap.Recipe.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.IngredientGroups;
public class IngredientGroupService : UseCaseService, IIngredientGroupService
{
    private readonly IRecipeDbContext _recipeDbContext;
    private readonly IMapper _mapper;

    public IngredientGroupService(
        IRecipeDbContext recipeDbContext,
        IMapper mapper)
    {
        _recipeDbContext = recipeDbContext;
        _mapper = mapper;
    }

    public async Task<List<IngredientGroupOutputDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var ingredientGroups = await _recipeDbContext.IngredientGroup.ToListAsync(cancellationToken);

        var output = _mapper.Map<List<IngredientGroupOutputDto>>(ingredientGroups);

        return output;
    }
}
