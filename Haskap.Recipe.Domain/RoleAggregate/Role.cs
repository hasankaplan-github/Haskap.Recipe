using Ardalis.GuardClauses;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.RoleAggregate.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.Recipe.Domain.RoleAggregate;
public class Role : AggregateRoot
{
    public string Name { get; private set; }

    private List<Permission> _permissions = new();
    public IReadOnlyList<Permission> Permissions => _permissions.AsReadOnly();

    private Role()
    {
    }

    public Role(Guid id, string name, DbSet<Role> roleDbSet)
        : base(id)
    {
        SetName(name, roleDbSet);
    }

    public void SetName(string name, DbSet<Role> roleDbSet)
    {
        Guard.Against.NullOrWhiteSpace(name);

        var exist = roleDbSet.Where(x => x.Name.ToLower().Equals(name.ToLower()))
            .Any();

        if (exist)
        {
            throw new DuplicateRoleNameException();
        }

        Name = name;
    }

    public void AddPermission(string permissionName)
    {
        _permissions.Add(new Permission(permissionName));
    }

    public void RemovePermission(string permissionName)
    {
        var toBeRemoved = _permissions.FirstOrDefault(x => x.Name.Equals(permissionName));
        _permissions.Remove(toBeRemoved);
    }

    public void RemovePermission(Permission permission)
    {
        _permissions.Remove(permission);
    }
}
