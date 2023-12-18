using Haskap.DddBase.Domain.Providers;
using Haskap.Recipe.Application.Dtos;
using Haskap.Recipe.Ui.MvcWebUi.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Haskap.DddBase.Presentation.CustomAuthorization;
using Haskap.Recipe.Domain.UserAggregate;
using Haskap.Recipe.Application.Contracts;

namespace Haskap.Recipe.Ui.MvcWebUi.ViewComponents.Role;

public class UpdatePermissions : ViewComponent
{
    private readonly IPermissionProvider _permissionProvider;
    private readonly IRoleService _roleService;

    public UpdatePermissions(
        IPermissionProvider permissionProvider, IRoleService roleService)
    {
        _permissionProvider = permissionProvider;
        _roleService = roleService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid roleId, CancellationToken cancellationToken)
    {
        ViewBag.RoleId = roleId;

        var permissions = await _roleService.GetPermissionsAsync(roleId, cancellationToken);

        ViewBag.SelectedPermissions = permissions;

        return View(_permissionProvider.GetAllPermissions());
    }
}
