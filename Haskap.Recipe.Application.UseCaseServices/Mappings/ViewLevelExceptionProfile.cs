using AutoMapper;
using Haskap.Recipe.Application.Dtos.ViewLevelExceptions;
using Haskap.Recipe.Domain.ViewLevelExceptionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Mappings;

internal class ViewLevelExceptionProfile : Profile
{
    public ViewLevelExceptionProfile()
    {
        CreateMap<ViewLevelException, ViewLevelExceptionOutputDto>();
    }
}
