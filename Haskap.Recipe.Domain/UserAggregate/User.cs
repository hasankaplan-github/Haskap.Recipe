using Ardalis.GuardClauses;
using Haskap.Recipe.Domain.Shared.Consts;
using Haskap.DddBase.Domain.Attributes.AuditHistoryLogAttributes;
using Haskap.Recipe.Domain.Common;
using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Haskap.Recipe.Domain.UserAggregate.Exceptions;
using System.Threading;
using Haskap.DddBase.Utilities.Guids;

namespace Haskap.Recipe.Domain.UserAggregate;

[AddAuditHistoryLog]
public class User : AggregateRoot, ISoftDeletable
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Credentials Credentials { get; private set; }
    public string? SystemTimeZoneId { get; private set; }

    private List<Permission> _permissions = new();
    public IReadOnlyList<Permission> Permissions => _permissions.AsReadOnly();

    private List<UserRole> _roles = new();
    public IReadOnlyList<UserRole> Roles => _roles.AsReadOnly();

    public bool IsDeleted { get; set; }

    private User()
    { }

    public User(Guid id, string firstName, string lastName, Credentials credentials, string? systemTimeZoneId, DbSet<User> userDbSet)
        : base(id)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetCredentials(credentials, userDbSet);
        SetSystemTimeZoneId(systemTimeZoneId);
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

    public void SetFirstName(string firstName)
    {
        Guard.Against.NullOrWhiteSpace(firstName, message: "İsim boş olamaz!");
        Guard.Against.InvalidInput(firstName, nameof(firstName), x => x.Length<=UserConsts.MaxFirstNameLength, message: "İsim 100 karakterden fazla olamaz!");
        FirstName = firstName;
    }

    public void SetLastName(string lastName)
    {
        Guard.Against.NullOrWhiteSpace(lastName, message: "Soyisim boş olamaz!");
        Guard.Against.InvalidInput(lastName, nameof(lastName), x => x.Length<= UserConsts.MaxFirstNameLength, message: "Soyisim 100 karakterden fazla olamaz!");
        LastName = lastName;
    }

    public void SetCredentials(Credentials credentials, DbSet<User> userDbSet)
    {
        Guard.Against.Null(credentials);

        var duplicateUserName = userDbSet
            .Where(x => x.Id != Id && x.Credentials.UserName.ToLower().Equals(credentials.UserName.ToLower()))
            .Any();

        if (duplicateUserName)
        {
            throw new DuplicateUserNameException();
        }

        Credentials = credentials;
    }

    public void SetSystemTimeZoneId(string? systemTimeZoneId)
    {
        if (systemTimeZoneId is not null)
        {
            Guard.Against.InvalidInput(systemTimeZoneId, nameof(systemTimeZoneId), x => x.Length <= UserConsts.MaxSystemTimeZoneIdLength, "SystemTimeZoneId 150 karakterden fazla olamaz!");
        }

        SystemTimeZoneId = systemTimeZoneId;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    public void AddRole(Guid roleId)
    {
        _roles.Add(new UserRole(GuidGenerator.CreateSimpleGuid()) { RoleId = roleId, UserId = Id });
    }

    public void RemoveRole(Guid roleId)
    {
        var toBeRemoved = _roles.Where(x => x.RoleId == roleId).First();
        _roles.Remove(toBeRemoved);
    }

    public void RemoveRole(UserRole role)
    {
        _roles.Remove(role);
    }
}
