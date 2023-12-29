using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Accounts;
using Haskap.Recipe.Application.Dtos.Common;
using Haskap.Recipe.Application.Dtos.Common.DataTable;
using Haskap.Recipe.Application.Dtos.Recipes;
using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Domain.Providers;
using Haskap.Recipe.Domain.RecipeAggregate;
using Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;
using Haskap.Recipe.Ui.MvcWebUi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Haskap.Recipe.Ui.MvcWebUi.Controllers;

[Authorize]
public class RecipeController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IRecipeService _recipeService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;
    private readonly IAccountService _accountService;
    private readonly IMultiUserGlobalQueryFilterProvider _multiUserFilter;
    private readonly StepPicturesSettings _stepPicturesSettings;

    public RecipeController(
        ICategoryService categoryService,
        IRecipeService recipeService,
        IWebHostEnvironment webHostEnvironment,
        IIsDraftGlobalQueryFilterProvider isDraftFilter,
        IAccountService accountService,
        IMultiUserGlobalQueryFilterProvider multiUserFilter,
        IOptions<StepPicturesSettings> stepPicturesSettingsOptions)
    {
        _categoryService = categoryService;
        _recipeService = recipeService;
        _webHostEnvironment = webHostEnvironment;
        _isDraftFilter = isDraftFilter;
        _accountService = accountService;
        _multiUserFilter = multiUserFilter;
        _stepPicturesSettings = stepPicturesSettingsOptions.Value;
    }

    

    [AllowAnonymous]
    public async Task<IActionResult> Search(SearchInputDto searchInputDto, CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        ViewBag.SearchInputDto = searchInputDto;

        using var _ = _multiUserFilter.Disable();

        var searchOutput = await _recipeService.SearchAsync(searchInputDto, cancellationToken);

        var pagination = new Pagination(searchInputDto.PageSize, searchInputDto.CurrentPageIndex, searchOutput.FilteredCount);
        ViewBag.Pagination = pagination;

        return View(searchOutput.Recipes);
    }


    [AllowAnonymous]
    [HttpGet("{slug}")]
    public async Task<IActionResult> Detail(string slug, CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        using var _ = _multiUserFilter.Disable();

        var recipe = await _recipeService.GetRecipeForDetailWiewAsync(slug, cancellationToken);

        return View(recipe);
    }

    [HttpGet("Preview/{slug}")]
    public async Task<IActionResult> Preview(string slug, CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        using var _ = _multiUserFilter.Disable();

        var recipe = await _recipeService.GetRecipeForPreviewWiewAsync(slug, cancellationToken);

        return View(recipe);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        using var _ = _multiUserFilter.Disable();

        var randomRecipes = await _recipeService.GetRandomRecipesAsync(2, cancellationToken);

        ViewBag.MostViewedRecipes = await _recipeService.GetMostViewedRecipiesAsync(6, cancellationToken);

        return View(randomRecipes);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        ViewBag.Categories = (await _categoryService.GetAllAsync(cancellationToken))
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
            .ToList();
        
        return View();
    }

    [HttpPost]
    public async Task<CreateAsDraftOutputDto> CreateAsDraft(CreateAsDraftInputDto inputDto, IFormFile formFile, CancellationToken cancellationToken = default)
    {
        var fileInputDto = new FileInputDto
        {
            ContentLength = formFile.Length,
            OriginalName = formFile.FileName
        };

        using (var memoryStream = new MemoryStream())
        {
            await formFile.CopyToAsync(memoryStream);
            fileInputDto.Content = memoryStream.ToArray();
        }

        return await _recipeService.CreateAsDraftAsync(inputDto, fileInputDto, _webHostEnvironment.WebRootPath, cancellationToken);
    }

    public async Task<IActionResult> Edit(Guid recipeId, CancellationToken cancellationToken = default)
    {
        ViewBag.RecipeId = recipeId;

        return View();
    }

    [HttpPut]
    public async Task Activate(Guid id, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        await _recipeService.ActivateAsync(id, cancellationToken);
    }

    [HttpPut]
    public async Task MarkAsDraft(Guid id, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        await _recipeService.MarkAsDraftAsync(id, cancellationToken);
    }

    [HttpDelete]
    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        await _recipeService.MarkAsDeletedAsync(id, cancellationToken);
    }

    [HttpPut]
    public async Task Update(Guid id, Application.Dtos.Recipes.UpdateInputDto inputDto, IFormFile formFile, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        FileInputDto? fileInputDto = null;

        if (formFile is not null)
        {
            fileInputDto = new FileInputDto
            {
                ContentLength = formFile.Length,
                OriginalName = formFile.FileName
            };

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                fileInputDto.Content = memoryStream.ToArray();
            }
        }

        await _recipeService.UpdateAsync(id, inputDto, fileInputDto, _webHostEnvironment.WebRootPath, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> LoadBasicInfoViewComponent(Guid recipeId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Recipe.BasicInfoViewComponent), new { recipeId });
    }

    [HttpGet]
    public async Task<IActionResult> LoadIngredientsViewComponent(Guid recipeId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Recipe.IngredientsViewComponent), new { recipeId });
    }

    [HttpDelete]
    public async Task DeleteIngredient(Guid recipeId, Guid ingredientId, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;
        
        await _recipeService.DeleteIngredientAsync(recipeId, ingredientId, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> LoadNewIngredientModalContentViewComponent(Guid recipeId, CancellationToken cancellationToken = default)
    {
        return ViewComponent(typeof(ViewComponents.Recipe.NewIngredientModalContentViewComponent), new { recipeId });
    }

    [HttpPost]
    public async Task SaveNewIngredient(SaveNewIngredientInputDto inputDto, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        await _recipeService.SaveNewIngredientAsync(inputDto, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> LoadUpdateIngredientModalContentViewComponent(Guid recipeId, Guid ingredientId, CancellationToken cancellationToken = default)
    {
        return ViewComponent(typeof(ViewComponents.Recipe.UpdateIngredientModalContentViewComponent), new { recipeId, ingredientId });
    }

    [HttpPut]
    public async Task UpdateIngredient(UpdateIngredientInputDto inputDto, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;
        
        await _recipeService.UpdateIngredientAsync(inputDto, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> LoadStepsViewComponent(Guid recipeId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Recipe.StepsViewComponent), new { recipeId });
    }

    [HttpGet]
    public async Task<IActionResult> LoadNewStepModalContentViewComponent(Guid recipeId, CancellationToken cancellationToken = default)
    {
        return ViewComponent(typeof(ViewComponents.Recipe.NewStepModalContentViewComponent), new { recipeId });
    }

    [HttpPost]
    public async Task SaveNewStep(SaveNewStepInputDto inputDto, List<IFormFile> formFiles, CancellationToken cancellationToken = default)
    {
        var pictureFiles = new List<FileInputDto>();

        if (formFiles?.Any() == true)
        {
            var picturesTasks = formFiles.AsParallel()
                .Select(async x =>
                {
                    var fileInputDto = new FileInputDto
                    {
                        ContentLength = x.Length,
                        OriginalName = x.FileName
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await x.CopyToAsync(memoryStream);
                        fileInputDto.Content = memoryStream.ToArray();
                    }

                    return fileInputDto;
                });

            pictureFiles = (await Task.WhenAll(picturesTasks)).ToList();
        }

        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;
        
        await _recipeService.SaveNewStepAsync(inputDto, pictureFiles, _webHostEnvironment.WebRootPath, cancellationToken);
    }

    [HttpPut]
    public async Task IncreaseStepOrder(IncreaseStepOrderInputDto inputDto, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        await _recipeService.IncreaseStepOrderAsync(inputDto, cancellationToken);
    }

    [HttpPut]
    public async Task DecreaseStepOrder(DecreaseStepOrderInputDto inputDto, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        await _recipeService.DecreaseStepOrderAsync(inputDto, cancellationToken);
    }

    [HttpDelete]
    public async Task DeleteStep(DeleteStepInputDto inputDto, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        await _recipeService.DeleteStepAsync(inputDto, _webHostEnvironment.WebRootPath, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> LoadToolbarViewComponent(Guid recipeId, CancellationToken cancellationToken = default)
    {
        return ViewComponent(typeof(ViewComponents.Recipe.ToolbarViewComponent), new { recipeId });
    }

    [HttpGet]
    public async Task<IActionResult> LoadUpdateStepModalContentViewComponent(Guid recipeId, Guid stepId, CancellationToken cancellationToken = default)
    {
        return ViewComponent(typeof(ViewComponents.Recipe.UpdateStepModalContentViewComponent), new { recipeId, stepId });
    }

    [HttpPut]
    public async Task UpdateStep(UpdateStepInputDto inputDto, List<IFormFile> formFiles, CancellationToken cancellationToken = default)
    {
        var pictureFiles = new List<FileInputDto>();

        if (formFiles?.Any() == true)
        {
            var picturesTasks = formFiles.AsParallel()
                .Select(async x =>
                {
                    var fileInputDto = new FileInputDto
                    {
                        ContentLength = x.Length,
                        OriginalName = x.FileName
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await x.CopyToAsync(memoryStream);
                        fileInputDto.Content = memoryStream.ToArray();
                    }

                    return fileInputDto;
                });

            pictureFiles = (await Task.WhenAll(picturesTasks)).ToList();
        }

        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        await _recipeService.UpdateStepAsync(inputDto, pictureFiles, _webHostEnvironment.WebRootPath, cancellationToken);
    }

    public async Task<IActionResult> EditorSearch(CancellationToken cancellationToken = default)
    {
        ViewBag.BaseFolderPath = _stepPicturesSettings.FolderName;

        return View();
    }

    [HttpPost]
    public async Task<JsonResult> EditorSearch(EditorSearchInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var allPermissions = await _accountService.GetAllPermissionsAsync(new GetAllPermissionsInputDto { UserId = userId }, cancellationToken);

        using var __ = allPermissions.Contains(Permissions.Recipe.Admin) ? _multiUserFilter.Disable() : null;

        var result = await _recipeService.EditorSearchAsync(inputDto, jqueryDataTableParam, cancellationToken);

        return Json(result);
    }
}
