using System.Net;
using System.Net.Mail;

namespace EcommerceMvc.Services;

public sealed class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(
        IConfiguration configuration,
        IWebHostEnvironment environment,
        ILogger<EmailSender> logger)
    {
        _configuration = configuration;
        _environment = environment;
        _logger = logger;
    }

    public async Task SendPasswordResetLinkAsync(string email, string resetUrl)
    {
        var smtpSection = _configuration.GetSection("Smtp");
        var host = smtpSection["Host"];
        var from = smtpSection["From"];

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(from))
        {
            if (_environment.IsDevelopment())
            {
                _logger.LogWarning(
                    "SMTP no configurado. Enlace de recuperación para {Email}: {ResetUrl}",
                    email,
                    resetUrl);
            }
            else
            {
                _logger.LogError("SMTP no configurado. No se pudo enviar recuperación a {Email}.", email);
            }

            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(from, smtpSection["FromName"] ?? "EcommerceMVC"),
            Subject = "Recupera tu contraseña",
            Body = $"""
                   Hola,

                   Recibimos una solicitud para cambiar tu contraseña.
                   Abre el siguiente enlace para crear una nueva contraseña:

                   {resetUrl}

                   Si no solicitaste este cambio, ignora este correo.
                   """,
            IsBodyHtml = false
        };

        message.To.Add(email);

        using var client = new SmtpClient(host, smtpSection.GetValue("Port", 587))
        {
            EnableSsl = smtpSection.GetValue("EnableSsl", true)
        };

        var userName = smtpSection["UserName"];
        var password = smtpSection["Password"];

        if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
        {
            client.Credentials = new NetworkCredential(userName, password);
        }

        await client.SendMailAsync(message);
    }
}
