using System.Text;
using Application.Abstraction.Services;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Infrastructure.Services.EmailServices;

public class SendConfirmMessageEmailService : IEmailConfirmMessageService
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailSenderService _emailSenderService;
    private readonly IEmailServicesConfiguration _configuration;

    public SendConfirmMessageEmailService(UserManager<User> userManager, IEmailSenderService emailSenderService, IEmailServicesConfiguration configuration)
    {
        _userManager = userManager;
        _emailSenderService = emailSenderService;
        _configuration = configuration;
    }

    public async Task SendEmailConfirmMessageAsync(User user)
    {
        var token = await GenerateConfirmationToken(user);
        
        var confirmUrl = _configuration.ConfirmUrl + $"{user.Id}/{token}";
        var emailBody = GenerateEmailBody(user, confirmUrl);

        await _emailSenderService.SendEmailAsync("Identity Server", user.Email, "Confirm Email", emailBody);
    }

    private static string GenerateEmailBody(User user, string confirmUrl)
    {
        return $"<h1>Hello {user.FirstName}! You so cute! :3</h1></br>" +
               $"Please confirm your email address <a href={System.Text.Encodings.Web.HtmlEncoder.Default.Encode(confirmUrl)}>Confirm</a>";
    }

    private async Task<string> GenerateConfirmationToken(User user)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var tokenGeneratedBytes = Encoding.UTF8.GetBytes(code);
        var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

        return codeEncoded;
    }
}