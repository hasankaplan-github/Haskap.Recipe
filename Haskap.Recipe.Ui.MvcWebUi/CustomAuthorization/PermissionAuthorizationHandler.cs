using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Accounts;
using Haskap.Recipe.Domain;
using Haskap.DddBase.Domain.Exceptions;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Presentation;
using Haskap.DddBase.Presentation.CustomAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!Guid.TryParse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
        {
            context.Fail();
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        var permissions = await accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId });

        if (!permissions.Contains(requirement.Name))
        {
            var forbiddenOperationException = new ForbiddenOperationException(requirement.DisplayText ?? requirement.Name);
            context.Fail(new AuthorizationFailureReason(this, forbiddenOperationException.Message));

            throw forbiddenOperationException;
        }
        
        context.Succeed(requirement);
    }
}
