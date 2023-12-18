using AutoMapper;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.ViewLevelExceptions;
using Haskap.Recipe.Domain;
using Haskap.Recipe.Domain.ViewLevelExceptionAggregate;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.ViewLevelExceptions;
public class ViewLevelExceptionService : UseCaseService, IViewLevelExceptionService
{
    private readonly IRecipeDbContext _ajandaDbContext;
    private readonly IMapper _mapper;

    public ViewLevelExceptionService(
        IRecipeDbContext ajandaDbContext,
        IMapper mapper)
    {
        _ajandaDbContext = ajandaDbContext;
        _mapper = mapper;
    }

    

    public async Task DeleteViewLevelExceptionAsync(Guid id)
    {
        await _ajandaDbContext.ViewLevelException
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<ViewLevelExceptionOutputDto> GetViewLevelExceptionAsync(Guid id)
    {
        var exception = await _ajandaDbContext.ViewLevelException
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        var output = _mapper.Map<ViewLevelExceptionOutputDto>(exception);

        return output;
    }

    public async Task<Guid> SaveAndGetIdAsync(SaveAndGetIdInputDto inputDto)
    {
        var viewLevelException = new ViewLevelException(GuidGenerator.CreateSimpleGuid())
        {
            Message = inputDto.Message,
            StackTrace = inputDto.StackTrace
        };

        await _ajandaDbContext.ViewLevelException.AddAsync(viewLevelException);
        await _ajandaDbContext.SaveChangesAsync();

        return viewLevelException.Id;
    }
}
