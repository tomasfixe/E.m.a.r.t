using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using System.Globalization;
using E.m.a.r.t.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

// Cria o builder para configurar a aplicação
var builder = WebApplication.CreateBuilder(args);

// Obtém a string de conexão com o banco de dados a partir do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configura o contexto do Entity Framework Core para usar SQL Server com a string de conexão
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Adiciona páginas de erro detalhadas para ambiente de desenvolvimento com EF Core
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configura o Identity para autenticação e autorização, incluindo roles (funções)
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true; // Usuário precisa confirmar e-mail para logar
        options.Password.RequireNonAlphanumeric = false; // Senha não precisa de caractere especial
        options.Password.RequireUppercase = false; // Senha não precisa de letra maiúscula
        options.Password.RequiredLength = 6; // Tamanho mínimo da senha
    })
    .AddEntityFrameworkStores<ApplicationDbContext>() // Usa o EF para armazenar dados do Identity
    .AddDefaultTokenProviders(); // Adiciona geradores de tokens para recuperação de senha, confirmação, etc.

// Adiciona suporte para Razor Pages
builder.Services.AddRazorPages();

// Adiciona suporte para MVC (controllers + views)
builder.Services.AddControllersWithViews();

// Ativa o serviço de sessões para armazenar dados do usuário entre requisições
builder.Services.AddSession();

// Registra um serviço para envio de e-mails (exemplo: SendGrid)
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

// Configura o cookie de autenticação para redirecionar para a página de login quando necessário
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Define a rota padrão para login
});

// Constrói a aplicação com todas as configurações acima
var app = builder.Build();

// Configura middleware para o ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // Permite migrar o banco via navegador no dev
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Middleware para tratar exceções e redirecionar para página de erro
    app.UseHsts(); // Força o uso de HTTPS estrito em produção
}

app.UseHttpsRedirection(); // Redireciona todas as requisições HTTP para HTTPS
app.UseStaticFiles(); // Permite servir arquivos estáticos (css, js, imagens)

// Configura roteamento das requisições
app.UseRouting();

app.UseSession(); // Ativa o middleware para sessões

app.UseAuthentication(); // Ativa autenticação para verificar o usuário logado
app.UseAuthorization(); // Ativa autorização para proteger recursos

// Define a rota padrão para controllers MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapeia as Razor Pages configuradas
app.MapRazorPages();

// Configura a cultura padrão da aplicação (formato de datas, moedas, etc.)
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Executa o seed dos dados iniciais (como criação de roles e usuário admin)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InicializarAsync(services);
}

// Inicia o aplicativo, colocando ele para rodar e aceitar requisições
app.Run();
