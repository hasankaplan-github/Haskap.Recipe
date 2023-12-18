using AutoMapper;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Categories;
using Haskap.Recipe.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Categories;
public class CategoryService : UseCaseService, ICategoryService
{
    private readonly IRecipeDbContext _recipeDbContext;
    private readonly IMapper _mapper;

    public CategoryService(
        IRecipeDbContext recipeDbContext,
        IMapper mapper)
    {
        _recipeDbContext = recipeDbContext;
        _mapper = mapper;
    }

    public async Task<List<CategoryOutputDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await _recipeDbContext.Category.ToListAsync(cancellationToken);

        var output = _mapper.Map<List<CategoryOutputDto>>(categories);

        return output;
    }
}
