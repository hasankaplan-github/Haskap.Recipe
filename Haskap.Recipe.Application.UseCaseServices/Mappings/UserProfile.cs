using AutoMapper;
using Haskap.Recipe.Application.Dtos.Accounts;
using Haskap.Recipe.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Application.UseCaseServices.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UpdateAccountOutputDto>()
            .ForMember(x => x.UserName, x => x.MapFrom(y => y.Credentials.UserName));
    }
}
