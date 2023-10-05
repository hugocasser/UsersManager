using Domain.Model;

namespace Application.Abstraction.Services;

public interface IEmailConfirmMessageService
{
    public Task SendEmailConfirmMessageAsync(User user);
}