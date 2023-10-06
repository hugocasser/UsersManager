using MailKit.Net.Smtp;

namespace Application.Abstraction.Repositories;

public interface ISmtpClientService : ISmtpClient
{
    public void Connect();
}