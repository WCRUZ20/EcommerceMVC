namespace Ecommerce.Services.Email;

public class DevelopmentEmailSender(ILogger<DevelopmentEmailSender> logger) : IEmailSender
{
    public Task SendPasswordResetAsync(string email, string resetLink, CancellationToken cancellationToken = default)
    {
        logger.LogWarning("Development password reset link for {Email}: {ResetLink}", email, resetLink);
        return Task.CompletedTask;
    }
}
