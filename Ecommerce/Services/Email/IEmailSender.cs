namespace Ecommerce.Services.Email;

public interface IEmailSender
{
    Task SendPasswordResetAsync(string email, string resetLink, CancellationToken cancellationToken = default);
}
