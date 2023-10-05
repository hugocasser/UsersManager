namespace Application.Abstraction.Services;

public interface IEmailSenderService
{
    public Task SendEmailAsync(string name, string email, string subject, string message);
}