using Haskap.DddBase.Domain.Providers;
using Haskap.Recipe.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Haskap.DddBase.Presentation.CustomAuthorization;
using Haskap.Recipe.Domain.UserAggregate;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Accounts;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Account;

public class UpdateRoles : ViewComponent
{
    private readonly IRoleService _roleService;
    private readonly IAccountService _accountService;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public UpdateRoles(
        IRoleService roleService,
        IAccountService accountService,
        ICurrentTenantProvider currentTenantProvider)
    {
        _roleService = roleService;
        _accountService = accountService;
        _currentTenantProvider = currentTenantProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid userId, CancellationToken cancellationToken)
    {
        ViewBag.UserId = userId;

        var roles = await _accountService.GetRolesAsync(new GetRolesInputDto{ UserId = userId }, cancellationToken);

        ViewBag.SelectedRoles = roles;

        return View(await _roleService.GetAllAsync(cancellationToken));
    }
}
