using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Models;

namespace E.m.a.r.t.Data
{
    /// <summary>
    /// Classe responsável por inicializar dados essenciais na base de dados,
    /// como roles e utilizadores predefinidos.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Método assíncrono que garante a criação das roles e do utilizador administrador.
        /// </summary>
        /// <param name="serviceProvider">Fornecedor de serviços para resolver dependências.</param>
        /// <returns>Task para aguardar a conclusão da operação.</returns>
        public static async Task InicializarAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Criar roles "Admin" e "User" se ainda não existirem
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Verificar se o utilizador administrador existe, caso contrário cria-o
            var emailAdmin = "projeto.emart@gmail.com";
            var adminPassword = "admin123";
            var admin = await userManager.FindByEmailAsync(emailAdmin);

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = emailAdmin,
                    Email = emailAdmin
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    // Confirmar email e atribuir role "Admin"
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(admin);
                    await userManager.ConfirmEmailAsync(admin, token);
                    await userManager.AddToRoleAsync(admin, "Admin");

                    // Adicionar entrada na tabela Utilizadores para o administrador
                    dbContext.Utilizadores.Add(new Utilizadores
                    {
                        Nome = "Administrador",
                        NIF = "211542989",
                        UserName = admin.UserName
                    });

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
