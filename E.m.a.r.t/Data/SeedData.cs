using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Models;

namespace E.m.a.r.t.Data
{
    public static class SeedData
    {
        public static async Task InicializarAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Criar roles se não existirem
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Criar utilizador admin se ainda não existir
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
                    // Confirmar email e associar role
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(admin);
                    await userManager.ConfirmEmailAsync(admin, token);
                    await userManager.AddToRoleAsync(admin, "Admin");

                    // Adiciona registo correspondente na tabela Utilizadores
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
