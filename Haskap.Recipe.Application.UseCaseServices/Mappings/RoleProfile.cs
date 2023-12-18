using AutoMapper;
using Haskap.Recipe.Application.Dtos.Roles;
using Haskap.Recipe.Domain.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Mappings;

internal class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleOutputDto>();
    }
}
