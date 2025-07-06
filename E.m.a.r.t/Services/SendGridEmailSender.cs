using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace E.m.a.r.t.Services
{
    /// <summary>
    /// Serviço para envio de e-mails usando a API SendGrid.
    /// </summary>
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor que recebe a configuração da aplicação para obter a chave da API.
        /// </summary>
        /// <param name="configuration">Configuração da aplicação.</param>
        public SendGridEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Envia um e-mail assíncrono usando SendGrid.
        /// </summary>
        /// <param name="email">Endereço de e-mail do destinatário.</param>
        /// <param name="subject">Assunto do e-mail.</param>
        /// <param name="htmlMessage">Conteúdo HTML da mensagem.</param>
        /// <returns>Tarefa assíncrona.</returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("projeto.emart@gmail.com", "E.M.A.R.T.");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: "", htmlMessage);
            await client.SendEmailAsync(msg);
        }
    }
}
