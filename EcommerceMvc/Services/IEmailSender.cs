namespace EcommerceMvc.Services;

public interface IEmailSender
{
    Task SendPasswordResetLinkAsync(string email, string resetUrl);
}
