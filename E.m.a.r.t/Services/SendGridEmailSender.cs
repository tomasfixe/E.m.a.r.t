using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
// Bibliotecas do SendGrid para envio de emails
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace E.m.a.r.t.Services
{
    
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Lê a chave da API do SendGrid a partir do appsettings.json
            var apiKey = _configuration["SendGrid:ApiKey"];

            var client = new SendGridClient(apiKey);

            // Define o remetente 
            var from = new EmailAddress("projeto.emart@gmail.com", "E.M.A.R.T.");

            // Define o destinatário
            var to = new EmailAddress(email);

            // Cria a mensagem do email
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: "", htmlMessage);

            // Envia a mensagem de forma assíncrona
            await client.SendEmailAsync(msg);
        }
    }
}