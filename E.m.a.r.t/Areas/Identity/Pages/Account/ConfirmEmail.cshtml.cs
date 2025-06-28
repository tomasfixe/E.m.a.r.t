using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;

namespace E.m.a.r.t.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        // Inje��o do UserManager para manipular utilizadores Identity
        private readonly UserManager<IdentityUser> _userManager;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Mensagem para mostrar o estado da confirma��o ao utilizador
        public string StatusMessage { get; set; }

        // M�todo acionado quando a p�gina � acedida via GET com userId e code na query string
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            // Se faltar userId ou c�digo, redireciona para a p�gina principal
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            // Tenta obter o utilizador a partir do userId
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Se n�o encontrar, retorna um erro 404 com mensagem personalizada
                return NotFound($"N�o foi poss�vel encontrar o utilizador com ID '{userId}'.");
            }

            // Decodifica o c�digo recebido, que vem em Base64URL (para seguran�a na URL)
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            // Confirma o email do utilizador com o c�digo decodificado
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            // Define a mensagem que ser� mostrada ao utilizador, conforme o resultado
            StatusMessage = result.Succeeded
                ? "O seu email foi confirmado com sucesso. Pode fazer login"
                : "Houve um erro ao confirmar o seu email.";

            // Mostra a p�gina com a mensagem apropriada
            return Page();
        }
    }
}
