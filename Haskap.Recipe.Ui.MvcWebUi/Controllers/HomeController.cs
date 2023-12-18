using Haskap.DddBase.Domain;
using Haskap.Recipe.Ui.MvcWebUi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Haskap.Recipe.Application.Contracts;

namespace Haskap.Recipe.Ui.MvcWebUi.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly IViewLevelExceptionService _viewLevelExceptionService;

    public HomeController(
        ILogger<HomeController> logger,
        IWebHostEnvironment environment,
        IViewLevelExceptionService viewLevelExceptionService)
    {
        _logger = logger;
        _environment = environment;
        _viewLevelExceptionService = viewLevelExceptionService;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error(Guid errorId)
    {
        var output = await _viewLevelExceptionService.GetViewLevelExceptionAsync(errorId);
        await _viewLevelExceptionService.DeleteViewLevelExceptionAsync(errorId);        

        return View(output);
    }

    
}
