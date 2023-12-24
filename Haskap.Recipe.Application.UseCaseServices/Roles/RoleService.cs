using AutoMapper;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Common;
using Haskap.Recipe.Application.Dtos.Roles;
using Haskap.Recipe.Domain;
using Haskap.Recipe.Domain.RoleAggregate;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Haskap.Recipe.Application.Dtos.Common.DataTable;

namespace Haskap.Recipe.Application.UseCaseServices.Roles;
public class RoleService : UseCaseService, IRoleService
{
    private readonly IRecipeDbContext _recipeDbContext;
    private readonly IMapper _mapper;

    public RoleService(
        IRecipeDbContext ajandaDbContext,
        IMapper mapper)
    {
        _recipeDbContext = ajandaDbContext;
        _mapper = mapper;
    }

    public async Task DeleteAsync(DeleteInputDto inputDto, CancellationToken cancellationToken)
    {
        var toBeDeleted = await _recipeDbContext.Role
            .Where(x=>x.Id == inputDto.RoleId)
            .FirstAsync(cancellationToken);

        _recipeDbContext.Role.Remove(toBeDeleted);
        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<RoleOutputDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var roles = await _recipeDbContext.Role
            .ToListAsync(cancellationToken);

        var output = _mapper.Map<List<RoleOutputDto>>(roles);

        return output;
    }

    public async Task SaveNewAsync(SaveNewInputDto inputDto, CancellationToken cancellationToken)
    {
        var newRole = new Role(
            GuidGenerator.CreateSimpleGuid(),
            inputDto.Name,
            _recipeDbContext.Role);

        await _recipeDbContext.Role.AddAsync(newRole, cancellationToken);
        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken)
    {
        var query = _recipeDbContext.Role.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;
        if (inputDto.Name is not null)
        {
            filtered = true;
            query = query.Where(x => x.Name.ToLower().Contains(inputDto.Name.ToLower()));
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
                    query = query.OrderBy(x => x.Name);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Name);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.Name);
        }

        var skip = jqueryDataTableParam.Start;
        var take = jqueryDataTableParam.Length;

        var roles = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        var roleOutputDtos = _mapper.Map<List<RoleOutputDto>>(roles);

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = roleOutputDtos
        };
    }

    public async Task<RoleOutputDto> GetByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _recipeDbContext.Role
            .Where(x => x.Id == roleId)
            .FirstAsync(cancellationToken);

        var output = _mapper.Map<RoleOutputDto>(role);

        return output;
    }

    public async Task UpdateAsync(UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var role = await _recipeDbContext.Role
            .Where(x => x.Id == inputDto.RoleId)
            .FirstAsync(cancellationToken);

        role.SetName(inputDto.NewName, _recipeDbContext.Role);

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePermissionsAsync(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        var role = await _recipeDbContext.Role
            .Where(x => x.Id == inputDto.RoleId)
            .FirstAsync(cancellationToken);

        var toBeDeleted = role.Permissions
            .IntersectBy(inputDto.UncheckedPermissions ?? Enumerable.Empty<string>(), x => x.Name)
            .ToList();

        foreach (var permission in toBeDeleted)
        {
            role.RemovePermission(permission);
        }


        var toBeAdded = (inputDto.CheckedPermissions ?? Enumerable.Empty<string>())
            .Except(role.Permissions.Select(x => x.Name))
            .ToList();
        
        foreach (var permissionName in toBeAdded)
        {
            role.AddPermission(permissionName);
        }

        await _recipeDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<PermissionOutputDto>> GetPermissionsAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _recipeDbContext.Role
           .Where(x => x.Id == roleId)
           .FirstAsync(cancellationToken);

        var output = _mapper.Map<List<PermissionOutputDto>>(role.Permissions);

        return output;
    }
}

