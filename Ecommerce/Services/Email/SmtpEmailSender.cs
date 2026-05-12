using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Ecommerce.Services.Email;

public class SmtpEmailSender(IOptions<EmailSettings> options) : IEmailSender
{
    private readonly EmailSettings settings = options.Value;

    public async Task SendPasswordResetAsync(string email, string resetLink, CancellationToken cancellationToken = default)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(settings.SenderEmail, settings.SenderName),
            Subject = "Restablece tu contraseña",
            Body = $"""
                Hola,

                Recibimos una solicitud para restablecer tu contraseña. Abre el siguiente enlace para crear una nueva contraseña:
                {resetLink}

                Si no solicitaste este cambio, puedes ignorar este mensaje.
                """,
            IsBodyHtml = false
        };
        message.To.Add(email);

        using var client = new SmtpClient(settings.Host, settings.Port)
        {
            EnableSsl = settings.EnableSsl
        };

        if (!string.IsNullOrWhiteSpace(settings.UserName))
        {
            client.Credentials = new NetworkCredential(settings.UserName, settings.Password);
        }

        await client.SendMailAsync(message, cancellationToken);
    }
}
