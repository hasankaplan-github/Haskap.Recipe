using Haskap.DddBase.Domain.Core;
using Haskap.DugunSalonu.Application.Dtos.Mappings;
using Haskap.DugunSalonu.Domain.Core.Shared;
using Haskap.DugunSalonu.Infrastructure.Data.DugunSalonuDbContext;
using Haskap.DugunSalonu.Presentation.WebApi;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<CateringPhotoSettings>(builder.Configuration.GetSection(CateringPhotoSettings.SectionName));

var connectionString = builder.Configuration.GetConnectionString("DugunSalonuConnectionString");
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    options.UseNpgsql(connectionString);
    //options.AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor<Guid?>>());
    //options.AddInterceptors(serviceProvider.GetRequiredService<AuditHistoryLogSaveChangesInterceptor<Guid?>>());
});

builder.Services.AddProviders();
builder.Services.AddUseCaseServices();
builder.Services.AddDomainServices();
builder.Services.AddEfInterceptors();

builder.Services.AddAutoMapper(typeof(ReservationProfile).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>()!;
        var exception = exceptionHandlerPathFeature.Error;

        var exceptionMessage = exception.Message /* is not null ? stringLocalizer[errorMessage] : null */ ;
        var exceptionStackTrace = exception.StackTrace;
        var httpStatusCode = exception switch
        {
            GeneralException generalException => generalException.HttpStatusCode,
            _ => HttpStatusCode.BadRequest
        };
        var errorEnvelope = Envelope.Error(exceptionMessage, exceptionStackTrace, exception.GetType().ToString(), httpStatusCode);

        app.Logger.LogError($"{JsonSerializer.Serialize(errorEnvelope)}{Environment.NewLine}" +
            $"=====================================================================================================================");

        if (app.Environment.IsDevelopment() == false)
        {
            errorEnvelope.SetExceptionStackTraceToNull();
        }

        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = Text.Plain;
        context.Response.StatusCode = (int)httpStatusCode;
        await context.Response.WriteAsJsonAsync(errorEnvelope);
    });
});




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
