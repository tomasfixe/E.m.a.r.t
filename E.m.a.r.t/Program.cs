using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using System.Globalization;
using E.m.a.r.t.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

// Cria o builder para configurar a aplica��o
var builder = WebApplication.CreateBuilder(args);

// Obt�m a string de conex�o com o banco de dados a partir do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configura o contexto do Entity Framework Core para usar SQL Server com a string de conex�o
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Adiciona p�ginas de erro detalhadas para ambiente de desenvolvimento com EF Core
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configura o Identity para autentica��o e autoriza��o, incluindo roles (fun��es)
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true; // Usu�rio precisa confirmar e-mail para logar
        options.Password.RequireNonAlphanumeric = false; // Senha n�o precisa de caractere especial
        options.Password.RequireUppercase = false; // Senha n�o precisa de letra mai�scula
        options.Password.RequiredLength = 6; // Tamanho m�nimo da senha
    })
    .AddEntityFrameworkStores<ApplicationDbContext>() // Usa o EF para armazenar dados do Identity
    .AddDefaultTokenProviders(); // Adiciona geradores de tokens para recupera��o de senha, confirma��o, etc.

// Adiciona suporte para Razor Pages
builder.Services.AddRazorPages();

// Adiciona suporte para MVC (controllers + views)
builder.Services.AddControllersWithViews();

// Ativa o servi�o de sess�es para armazenar dados do usu�rio entre requisi��es
builder.Services.AddSession();

// Registra um servi�o para envio de e-mails (exemplo: SendGrid)
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

// Configura o cookie de autentica��o para redirecionar para a p�gina de login quando necess�rio
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Define a rota padr�o para login
});

// Constr�i a aplica��o com todas as configura��es acima
var app = builder.Build();

// Configura middleware para o ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // Permite migrar o banco via navegador no dev
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Middleware para tratar exce��es e redirecionar para p�gina de erro
    app.UseHsts(); // For�a o uso de HTTPS estrito em produ��o
}

app.UseHttpsRedirection(); // Redireciona todas as requisi��es HTTP para HTTPS
app.UseStaticFiles(); // Permite servir arquivos est�ticos (css, js, imagens)

// Configura roteamento das requisi��es
app.UseRouting();

app.UseSession(); // Ativa o middleware para sess�es

app.UseAuthentication(); // Ativa autentica��o para verificar o usu�rio logado
app.UseAuthorization(); // Ativa autoriza��o para proteger recursos

// Define a rota padr�o para controllers MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapeia as Razor Pages configuradas
app.MapRazorPages();

// Configura a cultura padr�o da aplica��o (formato de datas, moedas, etc.)
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Executa o seed dos dados iniciais (como cria��o de roles e usu�rio admin)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InicializarAsync(services);
}

// Inicia o aplicativo, colocando ele para rodar e aceitar requisi��es
app.Run();
