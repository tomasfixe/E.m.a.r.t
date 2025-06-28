using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
// Bibliotecas do SendGrid para envio de emails
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace E.m.a.r.t.Services
{
    // Implementação do serviço de envio de e-mails usando SendGrid
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        // Injeta o IConfiguration para acessar as configurações do app (ex: appsettings.json)
        public SendGridEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método principal para envio de e-mail
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Obtém a chave da API do SendGrid configurada em appsettings.json, seção "SendGrid:ApiKey"
            var apiKey = _configuration["SendGrid:ApiKey"];

            // Cria o cliente do SendGrid usando a chave da API
            var client = new SendGridClient(apiKey);

            // Define o remetente do email (endereço e nome)
            var from = new EmailAddress("projeto.emart@gmail.com", "E.M.A.R.T.");

            // Define o destinatário do email
            var to = new EmailAddress(email);

            // Cria a mensagem a ser enviada (assunto, conteúdo em texto simples e HTML)
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: "", htmlMessage);

            // Envia o email de forma assíncrona via API do SendGrid
            await client.SendEmailAsync(msg);
        }
    }
}
