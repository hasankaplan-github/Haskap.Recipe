using AutoMapper;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Units;
using Haskap.Recipe.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Units;
public class UnitService : UseCaseService, IUnitService
{
    private readonly IRecipeDbContext _recipeDbContext;
    private readonly IMapper _mapper;

    public UnitService(
        IRecipeDbContext recipeDbContext,
        IMapper mapper)
    {
        _recipeDbContext = recipeDbContext;
        _mapper = mapper;
    }

    public async Task<List<UnitOutputDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var units = await _recipeDbContext.Unit.ToListAsync(cancellationToken);

        var output = _mapper.Map<List<UnitOutputDto>>(units);

        return output;
    }
}
