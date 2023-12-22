using Haskap.Recipe.Domain.Common;
using Haskap.Recipe.Infra.Db.Contexts.RecipeDbContext;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Haskap.Recipe.Ui.MvcWebUi;
using Haskap.DddBase.Domain;
using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using MediatR;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Haskap.DddBase.Infra.Providers;
using Haskap.DddBase.Presentation.Middlewares;
using Haskap.Recipe.Application.UseCaseServices.Mappings;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Haskap.Recipe.Ui.MvcWebUi.CustomAuthorization;
using Haskap.Recipe.Application.Contracts;
using Haskap.Recipe.Application.Dtos.ViewLevelExceptions;
using Haskap.Recipe.Ui.MvcWebUi.GlobalExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Haskap.Recipe.Application.UseCaseServices.Accounts;

//CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
CultureInfo cultureInfo = new CultureInfo("tr-tr"); //Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
cultureInfo.NumberFormat.CurrencyGroupSeparator = ",";
cultureInfo.NumberFormat.NumberGroupSeparator = ",";
//CultureInfo.DefaultThreadCurrentCulture = ci;
//CultureInfo.DefaultThreadCurrentUICulture = ci;


var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService()
                                     ? AppContext.BaseDirectory : default    
};

var builder = WebApplication.CreateBuilder(options);

//builder.WebHost.UseUrls("http://localhost:5050");

// Add services to the container.
builder.Services.Configure<StepPicturesSettings>(builder.Configuration.GetSection(StepPicturesSettings.SectionName));


builder.Services.AddPersistance(builder.Configuration);
//var connectionString = builder.Configuration.GetConnectionString("DugunSalonuConnectionString");
//builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
//{
//    options.UseNpgsql(connectionString);
//    options.UseSnakeCaseNamingConvention();
//    //options.AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor<Guid?>>());
//    //options.AddInterceptors(serviceProvider.GetRequiredService<AuditHistoryLogSaveChangesInterceptor<Guid?>>());
//});

builder.Services.AddBaseProviders();
builder.Services.AddProviders();
builder.Services.AddUseCaseServices();
builder.Services.AddDomainServices();
builder.Services.AddEfInterceptors();
builder.Services.AddExternalServices();
builder.Services.AddCustomAuthorization();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(AccountService).Assembly/*, typeof(CheckForDuplicateCreditCardTypeNameEventHandler).Assembly*/));
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);


//builder.Services.AddHttpClient<DugunSalonuHttpClient>(httpClient =>
//{
//    httpClient.BaseAddress = new Uri(builder.Configuration["DugunSalonuApiBaseUrl"]);
//    httpClient.DefaultRequestHeaders.Clear();
//    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "*/*");
//});
//builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});


builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        //options.AccessDeniedPath = "/Account/Login"; // default ayarı bu zaten
    });


builder.Host.UseWindowsService();

var app = builder.Build();

var mediator = app.Services.GetRequiredService<IMediator>();
MediatorWrapper.MediatorFunc = () => mediator;


//app.UseExceptionHandler("/Home/Error");

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();


app.UseSoftDelete();
app.UseLocalDateTimeProvider();
app.UseCurrentUserIdProvider();
app.UseVisitIdProvider();


//app.UseRequestLocalization("tr-TR");
app.UseRequestLocalization(x =>
{
    x.SupportedCultures = new List<CultureInfo>() { cultureInfo };
    x.SupportedUICultures = new List<CultureInfo>() { cultureInfo };
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Recipe}/{action=Create}/{id?}");

app.Run();
