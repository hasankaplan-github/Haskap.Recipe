using Haskap.DddBase.Domain.Providers;
using Haskap.DugunSalonu.Application.UseCaseServices;
using Haskap.DugunSalonu.Application.Contracts;
using Haskap.DugunSalonu.Domain.Providers;
using Haskap.DugunSalonu.Infrastructure.Providers;
using Haskap.DugunSalonu.Domain.Services;

namespace Haskap.DugunSalonu.Presentation.WebApi;

public static class ServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<CateringPhotoDomainService>();
        //services.AddTransient<PaymentCredentialsDomainService>();
    }

    public static void AddUseCaseServices(this IServiceCollection services)
    {
        //services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IReservationService, ReservationService>();
    }

    public static void AddProviders(this IServiceCollection services)
    {
        //services.AddScoped<CurrentUserProvider<User, Guid>>();
        //services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        //services.AddSingleton<IJwtProvider, JwtProvider>();
    }

    public static void AddEfInterceptors(this IServiceCollection services)
    {
        //services.AddScoped<AuditSaveChangesInterceptor<Guid?>>();
        //services.AddScoped<AuditHistoryLogSaveChangesInterceptor<Guid?>>();
    }
}