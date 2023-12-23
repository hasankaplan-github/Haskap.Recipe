using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Roles;
using Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;
using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Haskap.Recipe.Application.Dtos.Common.DataTable;

namespace Haskap.Recipe.Ui.MvcWebUi.Controllers;

[Authorize(Permissions.Recipe.Admin)]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public RoleController(
        IRoleService roleService,
        ICurrentTenantProvider currentTenantProvider)
    {
        _roleService = roleService;
        _currentTenantProvider = currentTenantProvider;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> Search(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default) 
    {
        var result = await _roleService.SearchAsync(inputDto, jqueryDataTableParam, cancellationToken);
        return Json(result);
    }

    [HttpPost]
    public async Task SaveNew(SaveNewInputDto inputDto, CancellationToken cancellationToken = default)
    {
        await _roleService.SaveNewAsync(inputDto, cancellationToken);
    }

    [HttpDelete]
    public async Task Delete(DeleteInputDto inputDto, CancellationToken cancellationToken = default)
    {
        await _roleService.DeleteAsync(inputDto, cancellationToken);
    }

    [HttpGet]
    public async Task<JsonResult> GetById(Guid roleId, CancellationToken cancellationToken = default)
    {
        var output = await _roleService.GetByIdAsync(roleId, cancellationToken);

        return Json(output);
    }

    [HttpPut]
    public async Task Update(UpdateInputDto inputDto, CancellationToken cancellationToken = default)
    {
        await _roleService.UpdateAsync(inputDto, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> LoadUpdatePermissionsViewComponent(Guid roleId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Role.UpdatePermissions), new { roleId });
    }

    [HttpPost]
    public async Task UpdatePermissions(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        if (_currentTenantProvider.IsHost == false)
        {
            inputDto.UncheckedPermissions?.RemoveAll(x => x.StartsWith("Permissions.Tenants"));
            inputDto.CheckedPermissions?.RemoveAll(x => x.StartsWith("Permissions.Tenants"));
        }

        await _roleService.UpdatePermissionsAsync(inputDto, cancellationToken);
    }

}

