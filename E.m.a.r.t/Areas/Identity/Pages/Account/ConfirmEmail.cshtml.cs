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
        
        private readonly UserManager<IdentityUser> _userManager;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Mensagem que mostra o estado (sucesso ou erro) da confirmação do email
        public string StatusMessage { get; set; }

        // Método chamado quando o utilizador entra nesta página via URL
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            // Se o userId ou o código forem nulos, redireciona para o index
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            // Procura o utilizador pelo ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Não foi possível encontrar o utilizador com ID '{userId}'.");
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            StatusMessage = result.Succeeded
                ? "O seu email foi confirmado com sucesso. Pode fazer login"
                : "Houve um erro ao confirmar o seu email.";

            return Page();
        }
    }
}
