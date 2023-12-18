using AutoMapper;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Accounts;
using Haskap.Recipe.Application.Dtos.Roles;
using Haskap.Recipe.Domain;
using Haskap.Recipe.Domain.UserAggregate;
using Haskap.Recipe.Domain.UserAggregate.Exceptions;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Domain.Providers;
using Microsoft.EntityFrameworkCore;
using Haskap.Recipe.Application.Dtos.Common.DataTable;

namespace Haskap.Recipe.Application.UseCaseServices.Accounts;
public class AccountService : UseCaseService, IAccountService
{
    private readonly IRecipeDbContext _recipeDbContext;
    private readonly IMapper _mapper;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly UserDomainService _userDomainService;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public AccountService(
        IRecipeDbContext ajandaDbContext,
        IMapper mapper,
        ICurrentUserIdProvider currentUserIdProvider,
        UserDomainService userDomainService,
        ICurrentTenantProvider currentTenantProvider)
    {
        _recipeDbContext = ajandaDbContext;
        _mapper = mapper;
        _currentUserIdProvider = currentUserIdProvider;
        _userDomainService = userDomainService;
        _currentTenantProvider = currentTenantProvider;
    }

    public async Task ChangePasswordAsync(ChangePasswordInputDto inputDto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(inputDto.CurrentPassword))
        {
            throw new CurrentPasswordEmptyException();
        }

        if (string.IsNullOrWhiteSpace(inputDto.NewPassword))
        {
            throw new NewPasswordEmptyException();
        }

        if (inputDto.NewPassword == inputDto.CurrentPassword)
        {
            throw new SamePasswordException();
        }

        if (inputDto.NewPassword != inputDto.NewPasswordConfirmation)
        {
            throw new PasswordConfirmationMismatchException();
        }

        var user = await _recipeDbContext.User
            .FindAsync(new object[] { _currentUserIdProvider.CurrentUserId!.Value }, cancellationToken);

        var hashedCurrentPassword = Password.ComputeHash(inputDto.CurrentPassword, user.Credentials.Password.Salt);
        if (user.Credentials.Password.HashedValue != hashedCurrentPassword)
        {
            throw new CurrentPasswordMismatchException();
        }

        var newPassword = new Password(inputDto.NewPassword, Salt.Generate());
        var newCredentials = new Credentials(user.Credentials.UserName, newPassword);

        user.SetCredentials(newCredentials, _recipeDbContext.User);
        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<LoginOutputDto> LoginAsync(LoginInputDto inputDto, CancellationToken cancellationToken = default)
    {
        var user = await _recipeDbContext.User
            .Where(x => x.Credentials.UserName == inputDto.UserName)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new WrongUserNameOrPasswordException();
        }

        var hashedPassword = Password.ComputeHash(inputDto.Password, user.Credentials.Password.Salt);
        if (hashedPassword != user.Credentials.Password.HashedValue)
        {
            throw new WrongUserNameOrPasswordException();
        }

        var output = new LoginOutputDto
        {
            UserId = user.Id,
            UserFirstName = user.FirstName,
            UserLastName = user.LastName,
            UserSystemTimeZoneId = user.SystemTimeZoneId
        };

        return output;
    }

    public async Task<HashSet<string>> GetAllPermissionsAsync(GetAllPermissionsInputDto inputDto, CancellationToken cancellationToken = default)
    {
        var allPermissions = await _userDomainService.GetAllPermissionsAsync(inputDto.UserId, cancellationToken);

        return allPermissions;
    }

    public async Task<HashSet<string>> GetUserPermissionsAsync(GetUserPermissionsInputDto inputDto, CancellationToken cancellationToken = default)
    {
        var userPermissions = await _userDomainService.GetUserPermissionsAsync(inputDto.UserId, cancellationToken);

        return userPermissions;
    }

    public async Task<UpdateAccountOutputDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _recipeDbContext.User
            .Where(x => x.Id == userId)
            .FirstAsync(cancellationToken);

        var output = _mapper.Map<UpdateAccountOutputDto>(user);

        return output;
    }

    public async Task UpdateAsync(Dtos.Accounts.UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserIdProvider.CurrentUserId.Value;

        var user = await _recipeDbContext.User
            .Where(x => x.Id == currentUserId)
            .FirstAsync(cancellationToken);

        var hashedCurrentPassword = Password.ComputeHash(inputDto.CurrentPassword, user.Credentials.Password.Salt);

        if (user.Credentials.Password.HashedValue != hashedCurrentPassword)
        {
            throw new CurrentPasswordMismatchException();
        }

        var newCredentials = new Credentials(inputDto.UserName, new Password(inputDto.CurrentPassword, Salt.Generate()));

        user.SetFirstName(inputDto.FirstName);
        user.SetLastName(inputDto.LastName);
        user.SetCredentials(newCredentials, _recipeDbContext.User);
        user.SetSystemTimeZoneId(inputDto.SystemTimeZoneId);

        await _recipeDbContext.SaveChangesAsync();
    }


    public async Task<JqueryDataTableResult> SearchAsync(Dtos.Accounts.SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken)
    {
        var query = (from user in _recipeDbContext.User
                     select new AccountOutputDto
                     {
                         Id = user.Id,
                         FirstName = user.FirstName, 
                         LastName = user.LastName,
                         SystemTimeZoneId = user.SystemTimeZoneId,
                         UserName = user.Credentials.UserName,
                     });

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;
        if (inputDto.FirstName is not null)
        {
            filtered = true;
            query = query.Where(x => x.FirstName.Contains(inputDto.FirstName));
        }

        if (inputDto.LastName is not null)
        {
            filtered = true;
            query = query.Where(x => x.LastName.Contains(inputDto.LastName));
        }

        if (inputDto.UserName is not null)
        {
            filtered = true;
            query = query.Where(x => x.UserName.Contains(inputDto.UserName));
        }


        if (filtered)
        {
            filteredCount = await query.CountAsync(cancellationToken);
        }

        if (jqueryDataTableParam.Order.Any())
        {
            var direction = jqueryDataTableParam.Order[0].Dir;
            var columnIndex = jqueryDataTableParam.Order[0].Column;

            if (columnIndex == 0)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.FirstName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.FirstName);
                }
            }
            else if (columnIndex == 1)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.LastName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.LastName);
                }
            }
            else if (columnIndex == 2)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.UserName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.UserName);
                }
            }
            else if (columnIndex == 3)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.SystemTimeZoneId);
                }
                else
                {
                    query = query.OrderByDescending(x => x.SystemTimeZoneId);
                }
            }
            else if (columnIndex == 4)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.UserName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.UserName);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.FirstName);
        }

        var skip = jqueryDataTableParam.Start;
        var take = jqueryDataTableParam.Length;

        var accountOutputDtos = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = accountOutputDtos
        };
    }

    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _recipeDbContext.User
            .Where(x => x.Id == userId)
            .FirstAsync(cancellationToken);

        user.MarkAsDeleted();

        await _recipeDbContext.SaveChangesAsync();
    }

    public async Task UpdatePermissionsAsync(Dtos.Accounts.UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        var user = await _recipeDbContext.User
            .Where(x => x.Id == inputDto.UserId)
            .FirstAsync(cancellationToken);

        var toBeDeleted = user.Permissions
            .IntersectBy(inputDto.UncheckedPermissions ?? Enumerable.Empty<string>(), x => x.Name)
            .ToList();

        foreach (var permission in toBeDeleted)
        {
            user.RemovePermission(permission);
        }


        var toBeAdded = (inputDto.CheckedPermissions ?? Enumerable.Empty<string>())
            .Except(user.Permissions.Select(x => x.Name))
            .ToList();

        foreach (var permissionName in toBeAdded)
        {
            user.AddPermission(permissionName);
        }

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<RoleOutputDto>> GetRolesAsync(GetRolesInputDto inputDto, CancellationToken cancellationToken)
    {
        var roles = (from user in _recipeDbContext.User
                     join userRole in _recipeDbContext.UserRole on user.Id equals userRole.UserId
                     join role in _recipeDbContext.Role on userRole.RoleId equals role.Id
                     where user.Id == inputDto.UserId
                     select new RoleOutputDto
                     {
                         Id = role.Id,
                         Name = role.Name
                     })
                    .ToList();

        return roles;
    }

    public async Task UpdateRolesAsync(Dtos.Accounts.UpdateRolesInputDto inputDto, CancellationToken cancellationToken)
    {
        var user = await _recipeDbContext.User
            .Include(x => x.Roles)
            .Where(x => x.Id == inputDto.UserId)
            .FirstAsync(cancellationToken);

        var toBeDeleted = user.Roles.IntersectBy(inputDto.UncheckedRoles ?? Enumerable.Empty<Guid>(), x => x.RoleId).ToList();

        foreach (var userRole in toBeDeleted)
        {
            user.RemoveRole(userRole);
        }


        var toBeAdded = (inputDto.CheckedRoles ?? Enumerable.Empty<Guid>()).Except(user.Roles.Select(x => x.RoleId));

        foreach (var roleId in toBeAdded)
        {
            user.AddRole(roleId);
        }

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }
}