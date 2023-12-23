using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.Common;
using Haskap.Recipe.Application.Dtos.Recipies;
using Haskap.Recipe.Domain.Providers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haskap.Recipe.Ui.MvcWebUi.Controllers;
public class RecipeController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IRecipeService _recipeService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IIsDraftGlobalQueryFilterProvider _isDraftFilter;

    public RecipeController(
        ICategoryService categoryService,
        IRecipeService recipeService,
        IWebHostEnvironment webHostEnvironment,
        IIsDraftGlobalQueryFilterProvider isDraftFilter)
    {
        _categoryService = categoryService;
        _recipeService = recipeService;
        _webHostEnvironment = webHostEnvironment;
        _isDraftFilter = isDraftFilter;
    }

    public IActionResult Index()
    {
        return View();
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
    public async Task<CreateAsDraftOutputDto> CreateAsDraft(CreateAsDraftInputDto inputDto, CancellationToken cancellationToken = default)
    {
        return await _recipeService.CreateAsDraftAsync(inputDto, cancellationToken);
    }

    public async Task<IActionResult> Edit(Guid recipeId, CancellationToken cancellationToken = default)
    {
        ViewBag.Categories = (await _categoryService.GetAllAsync(cancellationToken))
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
            .ToList();

        using var _ = _isDraftFilter.Disable();
        
        var recipeOutputDto = await _recipeService.GetByIdAsync(recipeId, cancellationToken);

        return View(recipeOutputDto);
    }

    [HttpPut]
    public async Task Activate(Guid id, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        await _recipeService.ActivateAsync(id, cancellationToken);
    }

    [HttpPut]
    public async Task MarkAsDraft(Guid id, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        await _recipeService.MarkAsDraftAsync(id, cancellationToken);
    }

    [HttpDelete]
    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        await _recipeService.MarkAsDeletedAsync(id, cancellationToken);
    }

    [HttpPut]
    public async Task Update(Guid id, UpdateInputDto inputDto, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        await _recipeService.UpdateAsync(id, inputDto, cancellationToken);
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

        await _recipeService.SaveNewStepAsync(inputDto, pictureFiles, _webHostEnvironment.WebRootPath, cancellationToken);
    }

    [HttpPut]
    public async Task IncreaseStepOrder(IncreaseStepOrderInputDto inputDto, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        await _recipeService.IncreaseStepOrderAsync(inputDto, cancellationToken);
    }

    [HttpPut]
    public async Task DecreaseStepOrder(DecreaseStepOrderInputDto inputDto, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

        await _recipeService.DecreaseStepOrderAsync(inputDto, cancellationToken);
    }

    [HttpDelete]
    public async Task DeleteStep(DeleteStepInputDto inputDto, CancellationToken cancellationToken = default)
    {
        using var _ = _isDraftFilter.Disable();

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

        await _recipeService.UpdateStepAsync(inputDto, pictureFiles, _webHostEnvironment.WebRootPath, cancellationToken);
    }
}
