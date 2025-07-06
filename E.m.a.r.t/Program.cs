using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using System.Globalization;
using E.m.a.r.t.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Obtem a string de conex�o � base de dados.
/// </summary>
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

/// <summary>
/// Configura o contexto da base de dados usando SQL Server.
/// </summary>
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

/// <summary>
/// Adiciona suporte a controllers e configura JSON para ignorar ciclos de refer�ncia.
/// </summary>
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

/// <summary>
/// Adiciona p�ginas de erro detalhadas em ambiente de desenvolvimento.
/// </summary>
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

/// <summary>
/// Configura Identity com suporte a roles e regras de password.
/// </summary>
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

/// <summary>
/// Adiciona suporte a Razor Pages e MVC.
/// </summary>
builder.Services.AddRazorPages();

/// <summary>
/// Ativa suporte a sess�es.
/// </summary>
builder.Services.AddSession();

/// <summary>
/// Regista o servi�o de envio de email (SendGrid).
/// </summary>
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

/// <summary>
/// Configura cookie de autentica��o com caminho de login personalizado.
/// </summary>
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});

var app = builder.Build();

/// <summary>
/// Configura middleware dependendo do ambiente.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

/// <summary>
/// Configura middlewares essenciais para seguran�a, est�ticos, autentica��o e sess�o.
/// </summary>
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

/// <summary>
/// Mapeia rotas para controllers API.
/// </summary>
app.MapControllers();

/// <summary>
/// Configura rota padr�o do MVC.
/// </summary>
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

/// <summary>
/// Mapeia Razor Pages, usadas para Identity.
/// </summary>
app.MapRazorPages();

/// <summary>
/// Define cultura padr�o para o sistema.
/// </summary>
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

/// <summary>
/// Executa seed inicial da base de dados.
/// </summary>
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InicializarAsync(services);
}

/// <summary>
/// Executa a aplica��o.
/// </summary>
app.Run();
