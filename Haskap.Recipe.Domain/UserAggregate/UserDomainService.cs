using Haskap.Recipe.Domain.RoleAggregate;
using Haskap.DddBase.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.UserAggregate;
public class UserDomainService : DomainService
{
    private readonly IRecipeDbContext _ajandaDbContext;

    public UserDomainService(IRecipeDbContext ajandaDbContext)
    {
        _ajandaDbContext=ajandaDbContext;
    }

    public async Task<HashSet<string>> GetAllPermissionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var allPermissions = (from user in _ajandaDbContext.User.Include(x => x.Roles).Where(x => x.Id == userId)
                              from role in _ajandaDbContext.Role.Where(x => user.Roles.Select(y => y.RoleId).Contains(x.Id)).DefaultIfEmpty()
                              select role.Permissions.Select(x => x.Name).Concat(user.Permissions.Select(x => x.Name)))
                              .SelectMany(x => x)
                              .ToHashSet();

        return allPermissions;
    }

    public async Task<HashSet<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _ajandaDbContext.User
           .Where(x => x.Id == userId)
           .FirstAsync(cancellationToken);

        var userPermissions = user.Permissions
            .Select(x => x.Name)
            .ToHashSet();

        return userPermissions;
    }

    public async Task<HashSet<string>> GetRolePermissionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var rolePermissions = (from user in _ajandaDbContext.User
                               join userRole in _ajandaDbContext.UserRole on user.Id equals userRole.UserId
                               join role in _ajandaDbContext.Role on userRole.RoleId equals role.Id
                               where user.Id == userId
                               select role.Permissions.Select(x => x.Name))
                            .SelectMany(x => x)
                            .ToHashSet();

        return rolePermissions;
    }
}
