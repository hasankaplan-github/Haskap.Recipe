using Haskap.DddBase.Domain;
using Haskap.DddBase.Presentation;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.ViewLevelExceptions;
using Haskap.Recipe.Application.UseCaseServices.ViewLevelExceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Text.Json;
using System;

namespace Haskap.Recipe.Ui.MvcWebUi.GlobalExceptionHandling;

public class DefaultExceptionHandler : IExceptionHandler
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<DefaultExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;


    public DefaultExceptionHandler(
        ILogger<DefaultExceptionHandler> logger,
        IWebHostEnvironment environment,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _environment = environment;
        _serviceScopeFactory = serviceScopeFactory;
    }


    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message /* is not null ? stringLocalizer[errorMessage] : null */ ;
        var exceptionStackTrace = exception.StackTrace;
        var httpStatusCode = exception switch
        {
            DomainException generalException => generalException.HttpStatusCode,
            _ => HttpStatusCode.BadRequest
        };
        var errorEnvelope = Envelope.Error(exceptionMessage, exceptionStackTrace, exception.GetType().ToString(), httpStatusCode);

        _logger.LogError($"{JsonSerializer.Serialize(errorEnvelope)}{Environment.NewLine}" +
            $"=====================================================================================================================");

        if (_environment.IsDevelopment() == false)
        {
            errorEnvelope.SetExceptionStackTraceToNull();
        }

        httpContext.Response.StatusCode = (int)httpStatusCode;

        if (UtilityMethods.IsAjaxRequest(httpContext.Request))
        {
            // using static System.Net.Mime.MediaTypeNames;
            httpContext.Response.ContentType = Text.Plain;
            await httpContext.Response.WriteAsJsonAsync(errorEnvelope);
        }
        else
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var viewLevelExceptionService = scope.ServiceProvider.GetService<IViewLevelExceptionService>();
            var inputDto = new SaveAndGetIdInputDto
            {
                Message = errorEnvelope.ExceptionMessage ?? string.Empty,
                StackTrace = errorEnvelope.ExceptionStackTrace
            };
            var errorId = await viewLevelExceptionService!.SaveAndGetIdAsync(inputDto);

            var controller = httpContext.GetRouteValue("controller")?.ToString();
            var action = httpContext.GetRouteValue("action")?.ToString();

            if (IsPublicArea(controller, action))
            {
                httpContext.Response.Redirect($"/Home/PublicError?errorId={errorId}");
            }
            else
            {
                httpContext.Response.Redirect($"/Home/Error?errorId={errorId}");
            }
        }

        return true;



        static bool IsPublicArea(string? controller, string? action)
        {
            return (controller == "Recipe" || controller is null) &&
                  (action == nameof(Controllers.RecipeController.Index) || 
                  action == nameof(Controllers.RecipeController.Detail) || 
                  action == nameof(Controllers.RecipeController.Search) ||
                  action == nameof(Controllers.RecipeController.MenuOfTheDay) ||
                  action is null);
        }
    }
}
