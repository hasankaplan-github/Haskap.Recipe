using AutoMapper;
using Haskap.Recipe.Application.Dtos.Categories;
using Haskap.Recipe.Domain.CategoryAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Mappings;

internal class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryOutputDto>();
    }
}
