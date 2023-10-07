using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.EmailServices;

public static class EmailServicesInjection
{
    public static IServiceCollection UseEmailServices(this IServiceCollection serviceCollection, IEmailServicesConfiguration configuration)
    {
        serviceCollection.AddSingleton(configuration);
        serviceCollection.AddTransient<IEmailConfirmMessageService, SendConfirmMessageEmailService>();
        serviceCollection.AddTransient<IEmailSenderService, EmailSenderService>();
        serviceCollection.AddTransient<ISmtpClientService, SmptClientService>();
        
        return serviceCollection;
    }
}