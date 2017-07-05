using CompetencyMatrix.Infrastructure;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using NLog;
using System;
using System.Threading.Tasks;

namespace CompetencyMatrix.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {

        ILoggerFactory _loggerFactory;
        public AuthMessageSender(ILoggerFactory loggerFactory) {
            _loggerFactory = loggerFactory;
        }

        public Task SendEmailAsync(string email, string subject, string messageText)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("", Startup.MailSettings.Login));
                message.To.Add(new MailboxAddress("", Startup.MailSettings.Login)); //temporary use stub
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = messageText
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(Startup.MailSettings.Host, Startup.MailSettings.Port, false);
                    client.Authenticate(Startup.MailSettings.Login, Startup.MailSettings.Password);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // Note: since we don't have an OAuth2 token, disable 	// the XOAUTH2 authentication mechanism.     client.Authenticate("anuraj.p@example.com", "password");
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<Program>();
                logger.LogError(LoggingEvents.SendEmail, ex, ex.Message);
            }

            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
