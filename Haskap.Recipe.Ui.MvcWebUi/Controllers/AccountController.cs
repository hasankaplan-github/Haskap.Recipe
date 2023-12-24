using Haskap.Recipe.Application.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Haskap.DddBase.Domain.Shared.Consts;
using Haskap.Recipe.Application.Dtos.Accounts;
using Haskap.DddBase.Domain.Providers;
using Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;
using Haskap.DddBase.Infra.Providers;
using Haskap.Recipe.Application.UseCaseServices.Accounts;
using Haskap.Recipe.Application.Dtos.Common.DataTable;

namespace Haskap.Recipe.Ui.MvcWebUi.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public AccountController(
        IAccountService accountService,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        _accountService = accountService;
        _currentUserIdProvider = currentUserIdProvider;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl = null, CancellationToken cancellationToken = default)
    {
        //var loginOutputDto = new LoginOutputDto { ReturnUrl = returnUrl ?? string.Empty };
        ViewBag.ReturnUrl = returnUrl ?? "/";

        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task Login(LoginInputDto inputDto, CancellationToken cancellationToken = default)
    {
        ViewBag.ReturnUrl = inputDto.ReturnUrl ?? "/";

        var output = await _accountService.LoginAsync(inputDto, cancellationToken);

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, inputDto.UserName),
                new Claim(ClaimTypes.GivenName, output.UserFirstName),
                new Claim(ClaimTypes.Surname, output.UserLastName),
                new Claim(ClaimTypes.NameIdentifier, output.UserId.ToString()),
                new Claim(LocalDateTimeProviderConsts.UserSystemTimeZoneIdClaimKey, output.UserSystemTimeZoneId ?? string.Empty)
            };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            //AllowRefresh = <bool>,
            // Refreshing the authentication session should be allowed.

            //ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(5), //.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            IsPersistent = false,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            IssuedUtc = DateTimeOffset.UtcNow
            // The time at which the authentication ticket was issued.

            //RedirectUri = <string>
            // The full path or absolute URI to be used as an http 
            // redirect response value.
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    [HttpPost]
    public async Task<IActionResult> Logout(string? returnUrl = null, CancellationToken cancellationToken = default)
    {
        if (returnUrl is null || returnUrl.Contains("Home/Error", StringComparison.OrdinalIgnoreCase))
        {
            returnUrl = "/";
        }
        
        if (User.Identity!.IsAuthenticated == false)
        {
            return LocalRedirect(returnUrl);
        }

        // Clear the existing external cookie
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login", new { returnUrl });  // LocalRedirect(returnUrl ?? "/");
    }

    public async Task<IActionResult> ChangePassword(CancellationToken cancellationToken)
    {
        return View();
    }

    [HttpPost]
    public async Task ChangePassword(ChangePasswordInputDto inputDto, CancellationToken cancellationToken)
    {
        await _accountService.ChangePasswordAsync(inputDto, cancellationToken);
    }

    public async Task<IActionResult> Update(CancellationToken cancellationToken)
    {
        ViewBag.SystemTimeZoneIds = TimeZoneInfo.GetSystemTimeZones()
            .Select(x => x.Id)
            .ToList();

        var output = await _accountService.GetByIdAsync(_currentUserIdProvider.CurrentUserId.Value, cancellationToken);

        return View(output);
    }

    [HttpPut]
    public async Task Update(UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        await _accountService.UpdateAsync(inputDto, cancellationToken);
    }

    [Authorize(Permissions.Recipe.Admin)]
    [HttpGet]
    public async Task<IActionResult> Accounts(CancellationToken cancellationToken)
    {
        return View();
    }

    [Authorize(Permissions.Recipe.Admin)]
    [HttpDelete]
    public async Task Delete(Guid userId, CancellationToken cancellationToken)
    {
        await _accountService.DeleteAsync(userId, cancellationToken);
    }

    [Authorize(Permissions.Recipe.Admin)]
    [HttpPost]
    public async Task<JsonResult> Search(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        var result = await _accountService.SearchAsync(inputDto, jqueryDataTableParam, cancellationToken);
        return Json(result);
    }

    [Authorize(Permissions.Recipe.Admin)]
    [HttpGet]
    public async Task<IActionResult> LoadUpdatePermissionsViewComponent(Guid userId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Account.UpdatePermissions), new { userId });
    }

    [Authorize(Permissions.Recipe.Admin)]
    [HttpPost]
    public async Task UpdatePermissions(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        await _accountService.UpdatePermissionsAsync(inputDto, cancellationToken);
    }

    [Authorize(Permissions.Recipe.Admin)]
    [HttpGet]
    public async Task<IActionResult> LoadUpdateRolesViewComponent(Guid userId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Account.UpdateRoles), new { userId });
    }

    [Authorize(Permissions.Recipe.Admin)]
    [HttpPost]
    public async Task UpdateRoles(UpdateRolesInputDto inputDto, CancellationToken cancellationToken)
    {
        await _accountService.UpdateRolesAsync(inputDto, cancellationToken);
    }
}
