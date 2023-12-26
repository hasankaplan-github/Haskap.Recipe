using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.ViewLevelExceptions;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Domain.Shared.Consts;
using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Haskap.Recipe.Ui.MvcWebUi.GlobalExceptionHandling;
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IViewLevelExceptionService viewLevelExceptionService,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            var exceptionMessage = exception.Message /* is not null ? stringLocalizer[errorMessage] : null */ ;
            var exceptionStackTrace = exception.StackTrace;
            var httpStatusCode = exception switch
            {
                DomainException generalException => generalException.HttpStatusCode,
                _ => HttpStatusCode.BadRequest
            };
            var errorEnvelope = Envelope.Error(exceptionMessage, exceptionStackTrace, exception.GetType().ToString(), httpStatusCode);

            logger.LogError($"{JsonSerializer.Serialize(errorEnvelope)}{Environment.NewLine}" +
                $"=====================================================================================================================");

            if (environment.IsDevelopment() == false)
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
                var inputDto = new SaveAndGetIdInputDto
                {
                    Message = errorEnvelope.ExceptionMessage ?? string.Empty,
                    StackTrace = errorEnvelope.ExceptionStackTrace
                };
                var errorId = await viewLevelExceptionService.SaveAndGetIdAsync(inputDto);

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
        }

        static bool IsPublicArea(string? controller, string? action)
        {
            return controller == "Recipe" &&
                  (action == nameof(Controllers.RecipeController.Index) || action == nameof(Controllers.RecipeController.Detail) || action == nameof(Controllers.RecipeController.Search));
        }
    }
}
