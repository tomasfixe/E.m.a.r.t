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

            // Cria os cargos de Admin e de Utilizador normal, se não existirem
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Cria um utilizador admin se ainda não existir
            var emailAdmin = "projeto.emart@gmail.com";
            var adminPassword = "E.M.A.R.T.dweb";
            var admin = await userManager.FindByEmailAsync(emailAdmin);

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = emailAdmin,
                    Email = emailAdmin,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                //  Só se criar com sucesso é que atribuímos role e criamos entrada
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");

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
