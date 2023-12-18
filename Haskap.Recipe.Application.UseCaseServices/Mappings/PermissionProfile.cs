using AutoMapper;
using Haskap.Recipe.Application.Dtos.Common;
using Haskap.Recipe.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Mappings;

internal class PermissionProfile : Profile
{
    public PermissionProfile()
    {
        CreateMap<Permission, PermissionOutputDto>();
    }
}
